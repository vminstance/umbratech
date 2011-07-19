// Helper functions

void GetCornerPosition(in float inData, out float outData[3])
{
    uint corner = abs(inData % 4);
    uint direction = abs(floor(inData / 4));

    if(direction == 0)
    {
        if(corner < 1)
		{
            outData[0] = 1;
            outData[1] = 1;
            outData[2] = 0;
			return;
        }
        if(corner < 2)
		{
            outData[0] = 1;
            outData[1] = 1;
            outData[2] = 1;
			return;
        }
        if(corner < 3)
		{
            outData[0] = 1;
            outData[1] = 0;
            outData[2] = 0;
			return;
        }
        if(corner < 4)
		{
            outData[0] = 1;
            outData[1] = 0;
            outData[2] = 1;
			return;
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
}
