using System.Collections;

public class Square
{
	bool HasMonster = false;
	public readonly float PositionX = 0.0f;
	public readonly float PositionZ = 0.0f;
	
	public Square(float x, float z)
	{
		PositionX = x;
		PositionZ = z;
	}
	
	public void SetMonster(bool set)
	{
		HasMonster = set;
	}
}
