using System;
using System.IO;

namespace HEC{

	class MEC{

		//--------------------------------------------------------------------------------------------------------------ham khoi tao MEC moi---------------------------------------------------------------------------------------------
		public void initial() {

			Console.WriteLine("\n\nInitializing MEC...\n\n");		//thong bao tao xong MECs

			//tao random so luong MEC
			random_Generator r = new random_Generator();
			int number_of_MEC = r.random_Number_in_range(8, 10);        		//tong so MEC
			
			//Console.WriteLine(number_of_MEC);								//debug - check number_of_MEC
		
			//int number_of_MEC = 6; 										//truong hop khai bao so luong MEC co dinh

			for(int i = 1; i <= number_of_MEC; i++){
				//Console.WriteLine(i);										//debug - check vong for
				
				//thong so cua MEC moi
				string mec_ID = i.ToString();								//tao thong so MEC_ID dong thoi la so luong MEC hien tai
				double CPU = r.random_Number_in_range(8, 17);				//tao thong so CPU va CPU_serving				

				//tao thu muc database cho MEC - ok
				string directory_Path = "./database/" + "MEC_" + mec_ID;	//ten thu muc data cho tung MEC
				Directory.CreateDirectory(directory_Path);					//tao thu muc (hoac viet de len thu muc cu)

				//tao file chua data MEC - ok
	            string file_Path = "./database/MEC_" + mec_ID + "/MEC_" + mec_ID + ".txt";		//vi tri file data (nam o trong thu muc MEC vua tao o tren)
	            string write_mec_data = CPU.ToString() + "\n" + "Connection\nMEC_Connection\n";		//du lieu cho tung MEC (CPU va CPU_serving)
	            File.WriteAllText(file_Path, write_mec_data);									//viet vao file

				//cap nhat connection vao database cua central_cloud (file txt cua Central_Cloud)
				Central_Cloud central = new Central_Cloud(); 
				central.update_MEC_Connection(mec_ID);						//update_MEC_Connection trong class Central_Cloud

				//cap nhat connection vao database cua cac MEC duoc ket noi (file txt cua cac MEC - bao gom ca MEC moi duoc khoi tao)
				if(i > 1){													//bo qua MEC dau tien - co moi 1 minh no thi ket noi voi MEC nao nua :))
					MEC mec_update_data = new MEC();						//tao 1 object MEC
					mec_update_data.update_MECs_Connection(mec_ID);			//update_MEC_Connection trong class MEC
				}else{
					File.AppendAllText(file_Path , "0\n" );					//ghi them tong so luong ket noi voi MEC khac
				}
			}

			Console.WriteLine("\n\nMECs initial done...\n\n");				//thong bao tao xong MECs

			Console.WriteLine("\n\nInitializing HEC...\n\n");				//thong bao tao xong MECs

			//----------------------------------Khoi tao HECs sau khi khoi tao MECs xong------------------------------------
			HEC create_HEC = new HEC();										//khoi tao object HEC
			create_HEC.initial(number_of_MEC);                      		//truyen so luong MEC vao ham initial trong class HEC

			Console.WriteLine("\n\nHECs initial done...\n\n");				//thong bao tao xong HECs
			//--------------------------------------------------------------------------------------------------------------

			//Sau khi khoi tao xong quay tro ve main_menu
			Console.WriteLine("\n\nInitial process done...\n\nPress '1' to go back main menu");

			char x='k';
        	int l=9, k = 0;

            Menu main_Menu = new Menu();

        	while(k != 1){
        		x = Console.ReadKey().KeyChar;    							//nhap (input) tuy chon tu ban phim

                Console.Clear();											//xoa man hinh hien thi
                Console.WriteLine("\n\nWe have done here!\n\nPress '1' to go back main menu");

                l = Convert.ToInt32(x);     								//chuyen input tu char sang int

                if (l == '1')   											//so sanh de chon cac lua chon trong menu
                {
                    k = 1;
                }
                else
                {
                    k = 0;
                }
        	}

            //lua chon tuy chon
            switch(l){
        		case '1': 
                    main_Menu.menu();       								//quay tro ve main_menu
                    break;    
        	}

		}

		//-------------------------------------------------------------ham cap nhat connection vao database cua cac MEC duoc ket noi (file txt cua cac MEC - bao gom ca MEC moi duoc khoi tao)-------------------------------------------------
		public void update_MECs_Connection(string mec_Number){			

			string mec_ID = mec_Number;										//thong so MEC_ID hien tai dong thoi la so luong MEC hien tai	
			int number_of_MEC = Convert.ToInt32(mec_ID);					//so luong MEC hien tai chuyen tu string sang int 				

			//Luu y: mec_ID (string type) = number_of_MEC (int type) = so luong MEC da duoc khoi tao den hien tai = ID cua MEC moi nhat duoc khoi tao

			string file_Path = "./database/MEC_" + mec_ID + "/MEC_" + mec_ID + ".txt";		//vi tri file data cua MEC moi khoi tao

			//tao random so luong ket noi toi cac MEC khac
			random_Generator r = new random_Generator();
			int number_of_MECs_connection = r.random_Number_in_range(1, number_of_MEC);		//tong so ket noi MEC

			File.AppendAllText(file_Path , number_of_MECs_connection.ToString() + "\n" );	//ghi them tong so luong ket noi voi MEC khac

			//test
			//int number_of_MECs_connection =	number_of_MEC - 1;							//tong so ket noi MEC co dinh - test debug						
			
			//tranh ket noi lap lai cung mot MEC
			int[] avoid_Repeating_MECs_ID = new int[number_of_MECs_connection];				//array cac ket noi da thiet lap - dùng để so sánh và tránh lặp kết nối		
			for(int m=0 ; m < number_of_MECs_connection; m++) 								//set gia tri cac phan tu trong mang = 0
			{
				avoid_Repeating_MECs_ID[m] = 0;
			}
			
			//Thiet lap cac ket noi random tu MEC moi duoc khoi tao -> cac MEC khac
			for(int i = 1; i <= number_of_MECs_connection ; i++){

				//tao MEC_ID bat ky ma MEC moi khoi tao se ket noi den
				random_Generator mec_will_connect = new random_Generator();
				int mec_ID_will_connect = mec_will_connect.random_Number_in_range(1, number_of_MECs_connection + 1);		

				int compare = 0;		

				if(i > 1){																	//bo qua ket noi dau tien - vi ket noi dau tien thi lam sao ma bi trung lap duoc :))				
					for(int j = 0; j < number_of_MECs_connection ; j++){					//kiem tra xem MEC chung ta dinh thiet lap ket noi da duoc ket noi tu truoc chua
						if( mec_ID_will_connect == avoid_Repeating_MECs_ID[j] ){
							compare = 1;
							break;
						}											
					}

					if(compare == 1){										//nếu kết nối bị lặp thì chạy lại một kết nối khác
						i--;
						continue;
					}

					else{													//nếu kết nối không bị lặp thì update vào database
						//update MEC duoc ket noi vao database cua MEC moi duoc khoi tao 
						File.AppendAllText(file_Path, mec_ID_will_connect.ToString() + "\n");

						//update MEC moi khoi tao vao database cua MEC duoc ket noi
						string mec_will_connect_file_Path = "./database/MEC_" + mec_ID_will_connect.ToString() + "/MEC_" + mec_ID_will_connect.ToString() + ".txt";				

						string[] lines = File.ReadAllLines(mec_will_connect_file_Path);		//cap nhat lai tong so luong ket noi voi MEC khac - trong file cua MEC duoc MEC moi khoi tao ket noi den
						File.WriteAllText(mec_will_connect_file_Path, "");					//xoa toan bo file
						for( int m = 0 ; m < lines.Length ; m++){							//cap nhat lai
							if( lines[m] == "MEC_Connection" ){
								lines[m+1] = (Convert.ToInt32(lines[m+1]) + 1).ToString() ;
								File.AppendAllText(mec_will_connect_file_Path, lines[m] + "\n" + lines[m+1] + "\n");
								m++;
							}else{
								File.AppendAllText(mec_will_connect_file_Path, lines[m] + "\n");
							}
						}

						File.AppendAllText(mec_will_connect_file_Path,  mec_ID + "\n");	//them MEC moi vao database duoc ket noi

						avoid_Repeating_MECs_ID[i-1] = mec_ID_will_connect;	
					}								
				}
				else{
					//update MEC duoc ket noi vao database cua MEC moi duoc khoi tao 
					File.AppendAllText(file_Path,  mec_ID_will_connect.ToString() + "\n");

					//update MEC moi khoi tao vao database cua MEC duoc ket noi
					string mec_will_connect_file_Path = "./database/MEC_" + mec_ID_will_connect.ToString() + "/MEC_" + mec_ID_will_connect.ToString() + ".txt";				

					string[] lines = File.ReadAllLines(mec_will_connect_file_Path);		//cap nhat lai tong so luong ket noi voi MEC khac - trong file cua MEC duoc MEC moi khoi tao ket noi den
					
					File.WriteAllText(mec_will_connect_file_Path, "");					//xoa toan bo file
					
					for( int m = 0 ; m < lines.Length ; m++){							//cap nhat lai
						if( lines[m] == "MEC_Connection" ){
							lines[m+1] = (Convert.ToInt32(lines[m+1]) + 1).ToString() ;
							File.AppendAllText(mec_will_connect_file_Path, lines[m] + "\n" + lines[m+1] + "\n");
							m++;
						}else{
							File.AppendAllText(mec_will_connect_file_Path, lines[m] + "\n");
						}
					}

					File.AppendAllText(mec_will_connect_file_Path,  mec_ID + "\n");	//them MEC moi vao database duoc ket noi

					avoid_Repeating_MECs_ID[i-1] = mec_ID_will_connect;	
				}
			}
		}

		//---------------------------------------------------------------------------------Ham tao ket noi voi HEC (khi HEC moi duoc add vao)-------------------------------------------------------------------------------
		public void update_HEC_Connection( string mec_Number , string hec_Number){
            string mec_ID = mec_Number;
            string hec_ID = hec_Number;

            string file_Path = "./database/MEC_" + mec_ID + "/MEC_" + mec_ID + ".txt";		//vi tri file data Central_Cloud      

            File.AppendAllText(file_Path , hec_ID + "\n");						                   							//add MEC_ID moi khoi tao vao cuoi file database cua Central_Cloud
		}		
	}
}