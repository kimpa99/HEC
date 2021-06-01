using System;
using System.IO;

namespace HEC{

	class Central_Cloud{

		//---------------------------------------------------------------------------------Ham tao ket noi voi MEC (khi MEC moi duoc add vao)-------------------------------------------------------------------------------
		public void update_MEC_Connection(string mec_Number){
            
            string file_Path = "./database/Central_Cloud.txt";		//vi tri file data Central_Cloud

            string mec_ID = mec_Number;                                 //MEC_ID cua MEC moi khoi tao
           		
            string[] lines = File.ReadAllLines(file_Path);              //cap nhat File Cloud

            File.WriteAllText(file_Path, "");

            for(int m = 0 ; m < lines.Length ; m++){                    //cap nhat tong so luong MEC

            	if( lines[m] == "MEC_Connection" ){

            		lines[m+1] = (Convert.ToInt32(lines[m+1]) + 1).ToString();

            		File.AppendAllText(file_Path, lines[m] + "\n" + lines[m+1] + "\n");

					m++;

            	}else{

            		File.AppendAllText(file_Path, lines[m] + "\n");
            		
            	}

            }

            File.AppendAllText(file_Path , mec_ID + "\n");     //add MEC_ID moi khoi tao vao cuoi file database cua Central_Cloud

		}

		//---------------------------------------------------------------------------------Ham khoi tao database Central_Cloud----------------------------------------------------------------------------------------------
		public void initial(){

            string file_Path = "./database/Central_Cloud.txt";      //vi tri file data Central_Cloud

            string write_cloud_data = "5000\nConnection\nMEC_Connection\n0\n";                                                   //kệ mẹ data cũ - bố mày viết đè lên hết (CPU)

            File.WriteAllText(file_Path , write_cloud_data);                                                //viet vao file database cua Cloud                                            //thong bao tao xong Central_Cloud
		}
	}
}