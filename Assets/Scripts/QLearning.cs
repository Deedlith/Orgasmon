using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QLearning
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public struct Couple
    {
        public int distance;
        public Direction direction;
    }

    public struct Coord
    {
        public int x;
        public int z;
    }

    // First int = distance, second int = Direction (action)
    int[,] _arrayReward = new int[10, 10];
    int[,] _arrayQuality = new int[10, 10];
    Dictionary<Couple, int> _dicoQuality = new Dictionary<Couple,int>();
    List<Movement> _listMovCurrent = new List<Movement>();

    Coord posLeft = new Coord();
    Coord posRight = new Coord();
    Coord posBack = new Coord();
    Coord posFront = new Coord();

    Field field = Field.Instance;

    int nbMovesLeft = 0;

    void InitArrayReward()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                _arrayReward[i, j] = -1;
            }
        }
    }

    void InitArrayQuality()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                _arrayQuality[i, j] = -1;
            }
        }
    }

    int GetPercentReward(int NbElem)
    {
        return 100 / NbElem * nbMovesLeft;
    }

    void SetReward(Coord pos, int nbElem, Movement lastMov, Movement dir)
    {
        MonoBehaviour.print("X : " + pos.x + " Z : " + pos.z);
        int reward = GetPercentReward(nbElem);
        if(pos.x - 1 >= 0 && pos.z < 10)
            _arrayReward[pos.x - 1, pos.z] = (reward > _arrayReward[pos.x - 1, pos.z]) ? reward : _arrayReward[pos.x - 1, pos.z];
        if(pos.x + 1 < 10 && pos.z < 10)
            _arrayReward[pos.x + 1, pos.z] = (reward > _arrayReward[pos.x + 1, pos.z]) ? reward : _arrayReward[pos.x + 1, pos.z];
        if (pos.z - 1 >= 0 && pos.x < 10)
            _arrayReward[pos.x, pos.z - 1] = (reward > _arrayReward[pos.x, pos.z - 1]) ? reward : _arrayReward[pos.x, pos.z - 1];;
        if (pos.z + 1 < 10 && pos.x < 0)
            _arrayReward[pos.x, pos.z + 1] = (reward > _arrayReward[pos.x, pos.z + 1]) ? reward : _arrayReward[pos.x, pos.z + 1];;

        if (nbMovesLeft > 0)
        {
            MonoBehaviour.print(lastMov.ToString());
            nbMovesLeft--;
            Coord newPosLeft = new Coord();
            Coord newPosRight = new Coord();
            Coord newPosBack = new Coord();
            Coord newPosFront = new Coord();

            newPosLeft.x = pos.x - 1;
            newPosLeft.z = pos.z;
            newPosRight.x = pos.x + 1;
            newPosRight.z = pos.z;
            newPosBack.x = pos.x;
            newPosBack.z = pos.z - 1;
            newPosFront.x = pos.x;
            newPosFront.z = pos.z + 1;
            
            if (lastMov == _listMovCurrent[nbMovesLeft])
            {
                MonoBehaviour.print("Change");
                SetReward(newPosBack, nbElem, _listMovCurrent[nbMovesLeft], dir);
                SetReward(newPosFront, nbElem, _listMovCurrent[nbMovesLeft], dir);
                /*newPosLeft.x = pos.x - 1;
                newPosLeft.z = pos.z;
                newPosRight.x = pos.x + 1;
                newPosRight.z = pos.z;
                newPosBack.x = pos.x;
                newPosBack.z = pos.z - 1;
                newPosFront.x = pos.x;
                newPosFront.z = pos.z + 1;*/
            }
            else
            {
                MonoBehaviour.print("CHANGE !");

                /*newPosLeft.x = pos.z - 1;
                newPosLeft.z = pos.x;
                newPosRight.x = pos.z + 1;
                newPosRight.z = pos.x;
                newPosBack.x = pos.z;
                newPosBack.z = pos.x - 1;
                newPosFront.x = pos.z;
                newPosFront.z = pos.x + 1;*/
                SetReward(newPosLeft, nbElem, _listMovCurrent[nbMovesLeft], dir);
                SetReward(newPosRight, nbElem, _listMovCurrent[nbMovesLeft], dir);
            }
        }
    }

    public Direction CalculateDirection(Monster ia, Monster target)
    {
        // On génère les array reward et quality pour cette situation précise
        GenerateArrayReward(target);
        GenerateArrayQuality(ia);

        // On trouve la meilleure qualité
        int qUp = -1; int qDown = -1; int qLeft = -1; int qRight = -1;
        int posX = Mathf.RoundToInt(ia.currentSquare.PositionX);
        int posZ = Mathf.RoundToInt(ia.currentSquare.PositionZ);
        if (posX - 1 >= 0)
            qLeft = _arrayQuality[posX, posZ];
        if (posX + 1 < 10)
            qRight = _arrayQuality[posX, posZ];
        if (posZ - 1 >= 0)
            qDown = _arrayQuality[posX, posZ];
        if (posZ + 1 < 10)
            qUp = _arrayQuality[posX, posZ];

        // On selectionne la direction
        if (qLeft > qRight && qLeft > qDown && qLeft > qUp)
            return Direction.Left;
        else if (qRight > qDown && qRight > qUp)
            return Direction.Right;
        else if (qUp > qDown)
            return Direction.Up;
        else
            return Direction.Down;
    }

    public void GenerateArrayReward(Monster target)
    {
        InitArrayReward();
        Coord posTarget = new Coord();
        posTarget.x = Mathf.RoundToInt(target.currentSquare.PositionX);
        posTarget.z = Mathf.RoundToInt(target.currentSquare.PositionZ);
        _arrayReward[posTarget.x, posTarget.z] = 100;
        MonoBehaviour.print("HERE " + posTarget.x + " - " + posTarget.z + " : " + _arrayReward[posTarget.x, posTarget.z]);
        nbMovesLeft = target.listMovements.Count-1;
        _listMovCurrent = target.listMovements;

        posLeft.x = posTarget.x - 1; posLeft.z = posTarget.z;
        posRight.x = posTarget.x + 1; posRight.z = posTarget.z;
        posBack.x = posTarget.x; posBack.z = posTarget.z - 1;
        posFront.x = posTarget.x; posFront.z = posTarget.z + 1;

        Movement lastMovement = target.listMovements.Last();
        Movement dir;

        for (int i = 0; i < 2; i++)
        {
            dir = (Movement) i;
            MonoBehaviour.print("DIR : " + dir.ToString());
            SetReward(posTarget, _listMovCurrent.Count, lastMovement, dir);
        }

        for (int i = 0; i < 10; i++)
        {
            string line = string.Empty;
            for (int j = 0; j < 10; j++)
            {
                string toConcat = "[" + _arrayReward[i, j] + "]";
                line = string.Concat(line, toConcat);
            }
            MonoBehaviour.print(line);
        }
    }

    public void GenerateArrayQuality(Monster m)
    {
        float gamma = 0.75f;

        InitArrayQuality();

        for (int i = 0; i < 50; i++)
        {
            // Set the current starting position
            int posX = Mathf.RoundToInt(m.currentSquare.PositionX);
            int posZ = Mathf.RoundToInt(m.currentSquare.PositionZ);

            do
            {
                /* Select an action
                int offsetX = Random.Range(0, 2);
                int offsetZ = Random.Range(0, 2);
                */
                // Choose a mouvement with the four possible direction
                // Do the mouvement
                int maxQ = 0;
                // Then calcul all the other movement quality, select the max and use it pour the true quality
                for (int j = 0; j < 4; j++)
                {

                }

                _arrayQuality[posX, posZ] = Mathf.RoundToInt(_arrayReward[posX, posZ] + gamma * maxQ);

            } while ((posX != posLeft.x && posZ != posLeft.z)
                || (posX != posRight.x && posZ != posRight.z)
                || (posX != posFront.x && posZ != posFront.z)
                || (posX != posBack.x && posZ != posBack.z));
        }

        MonoBehaviour.print("ARRAY QUALITY");
        for (int i = 0; i < 10; i++)
        {
            string line = string.Empty;
            for (int j = 0; j < 10; j++)
            {
                string toConcat = "[" + _arrayQuality[i, j] + "]";
                line = string.Concat(line, toConcat);
            }
            MonoBehaviour.print(line);
        }
    }
}
