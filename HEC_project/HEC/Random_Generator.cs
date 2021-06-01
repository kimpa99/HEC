using System;

namespace HEC{

	class random_Generator{

		//ham tao gia tri random trong khoang min - max
		public int random_Number_in_range(int min, int max){

			Random r = new Random();	//tao object tu class Random

			return r.Next(min, max);	// min <= so duoc tao < max
		}
	}
}