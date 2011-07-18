// Helper functions

float4 GetCornerPosition(in float inData)
{
    uint corner = abs(inData % 4);
    uint direction = abs(floor(inData / 4));

    switch(direction)
    {
		case 0:
		{
			switch(corner)
			{
				case 0:
					return float4(1, 1, 0, 0);
					break;

				case 1:
					return float4(1, 1, 1, 0);
					break;

				case 2:
					return float4(1, 0, 0, 0);
					break;

				case 3:
					return float4(1, 0, 1, 0);
					break;
			}
			break;
		}
	}
    //else if(direction == 1)
    //{
		//if(corner == 0)
		//{
            //return float4(0, 1, 1, 0);
        //}
		//else if(corner == 1)
		//{
            //return float4(0, 1, 0, 0);
        //}
		//else if(corner == 2)
		//{
            //return float4(0, 0, 1, 0);
        //}
		//else if(corner == 3)
		//{
            //return float4(0, 0, 0, 0);
        //}
    //}
    //else if(direction == 2)
    //{
		//if(corner == 0)
		//{
            //return float4(1, 1, 0, 0);
        //}
		//else if(corner == 1)
		//{
            //return float4(0, 1, 0, 0);
        //}
		//else if(corner == 2)
		//{
            //return float4(1, 1, 1, 0);
        //}
		//else if(corner == 3)
		//{
            //return float4(0, 1, 1, 0);
        //}
    //}
    //else if(direction == 3)
    //{
		//if(corner == 0)
		//{
            //return float4(0, 0, 0, 0);
        //}
		//else if(corner == 1)
		//{
            //return float4(1, 0, 0, 0);
        //}
		//else if(corner == 2)
		//{
            //return float4(0, 0, 1, 0);
        //}
		//else if(corner == 3)
		//{
            //return float4(1, 0, 1, 0);
        //}
    //}
    //else if(direction == 4)
    //{
		//if(corner == 0)
		//{
            //return float4(0, 1, 0, 0);
        //}
		//else if(corner == 1)
		//{
            //return float4(1, 1, 0, 0);
        //}
		//else if(corner == 2)
		//{
            //return float4(0, 0, 0, 0);
        //}
		//else if(corner == 3)
		//{
            //return float4(1, 0, 0, 0);
        //}
    //}
    //else if(direction == 5)
    //{
		//if(corner == 0)
		//{
            //return float4(1, 1, 1, 0);
        //}
		//else if(corner == 1)
		//{
            //return float4(0, 1, 1, 0);
        //}
		//else if(corner == 2)
		//{
            //return float4(1, 0, 1, 0);
        //}
		//else if(corner == 3)
		//{
            //return float4(0, 0, 1, 0);
        //}
    //}
	return float4(0, 0, 0, 0);
}