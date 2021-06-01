using System;
using System.IO;

namespace HEC{

	class HEC{

		//----------------------------------------------------------------------------------------------ham khoi tao HECs moi---------------------------------------------------------------------------------------------------
		public void initial(int number_of_MEC){

			int total_number_of_MECs = number_of_MEC;									//tong so MEC

			for(int mec = 1; mec <= total_number_of_MECs ; mec++){						//khoi tao HECs ben trong moi MEC

				//tao random so luong HECs ben trong moi MEC
				random_Generator r = new random_Generator();
				int number_of_HEC = r.random_Number_in_range(100, 150);        		//tong so HEC

				//int number_of_HEC = 6; 										//truong hop khai bao so luong HEC co dinh

				File.AppendAllText("./database/MEC_" + mec.ToString() + "/MEC_" + mec.ToString() + ".txt" , "HEC_Connection\n" + number_of_HEC.ToString() + "\n");	//ghi them dong HEC_Connection vao file MEC quan ly

				for(int i = 1; i <= number_of_HEC; i++){
					
					//thong so cua HEC moi
					string hec_ID = i.ToString();					//tao thong so HEC_ID dong thoi la so luong HEC hien tai
					int hec_CPU = r.random_Number_in_range(5 , 10);	//tao thong so hec_CPU

					//tao file chua data HEC
		            string hec_file_Path = "./database/MEC_" + mec.ToString() + "/HEC_" + hec_ID + ".txt";		//vi tri file data (nam o trong thu muc MEC ma HEC nam trong)
		            string write_hec_data = hec_CPU.ToString() + "\nConnection\nHEC_Connection\n";				//du lieu cho tung HEC (hec_CPU va hec_CPU_serving)
		            File.WriteAllText(hec_file_Path, write_hec_data);																					//viet vao file

					//cap nhat connection vao database cua MEC (file txt cua MEC lien quan)
					MEC mec_update = new MEC();														//tao object tu class MEC
					mec_update.update_HEC_Connection( mec.ToString() , hec_ID );					//update_HEC_Connection trong class MEC

					//cap nhat connection vao database cua cac HEC duoc ket noi (file txt cua cac HEC - bao gom ca HEC moi duoc khoi tao)
					if(i > 1){																		//bo qua HEC dau tien - co moi 1 minh no thi ket noi voi HEC nao nua :))
						HEC hec_update_data = new HEC();											//tao 1 object hec
						hec_update_data.update_HECs_Connection( mec.ToString() , hec_ID );								//update_HECs_Connection trong class HEC
					}else{
						File.AppendAllText(hec_file_Path , "0\n" );									//ghi them tong so luong ket noi voi HEC khac
					}
				}
			}
		}


		//----------------------------------------------Ham cap nhat connection vao database cua cac HEC duoc ket noi (file txt cua cac HEC - bao gom ca HEC moi duoc khoi tao)-----------------------------------------------
		public void update_HECs_Connection(string mec_Number , string hec_Number){			

			string mec_ID = mec_Number;						//MEC hien tai quan ly cac HEC
			string hec_ID = hec_Number;						//thong so hec_ID hien tai dong thoi la so luong HEC hien tai
			int number_of_HEC = Convert.ToInt32(hec_ID);	//so luong HEC hien tai chuyen tu string sang int 				

			//Luu y: hec_ID (string type) = number_of_HEC (int type) = so luong HEC da duoc khoi tao den hien tai = ID cua HEC moi nhat duoc khoi tao

			string file_Path = "./database/MEC_" + mec_ID + "/HEC_" + hec_ID + ".txt";		//vi tri file data cua HEC moi khoi tao

			//tao random so luong ket noi toi cac MEC khac
			random_Generator r = new random_Generator();
			int number_of_HECs_connection = r.random_Number_in_range(1, number_of_HEC);			//tong so ket noi HEC

			File.AppendAllText(file_Path , number_of_HECs_connection.ToString() + "\n");		//ghi them so luong HEC_Connection

			//test
			//int number_of_HECs_connection = number_of_HEC - 1;								//truong hop khai bao tong so ket noi HEC co dinh - test debug						
			
			//tranh ket noi lap lai cung mot HEC
			int[] avoid_Repeating_HECs_ID = new int[number_of_HECs_connection];				//array cac ket noi da thiet lap - dùng để so sánh và tránh lặp kết nối		
			for(int m=0; m < number_of_HECs_connection; m++) 								//set gia tri cac phan tu trong mang = 0
			{
				avoid_Repeating_HECs_ID[m] = 0;
			}
			
			//Thiet lap cac ket noi random tu HEC moi duoc khoi tao -> cac HEC khac
			for(int i = 1; i <= number_of_HECs_connection ; i++){

				//tao MEC_ID bat ky ma HEC moi khoi tao se ket noi den
				random_Generator hec_will_connect = new random_Generator();
				int hec_ID_will_connect = hec_will_connect.random_Number_in_range(1, number_of_HECs_connection + 1);		

				int compare = 0;				
				if(i > 1){																	//bo qua ket noi dau tien - vi ket noi dau tien thi lam sao ma bi trung lap duoc :))				
					for(int j = 0; j < number_of_HECs_connection ; j++){					//kiem tra xem HEC chung ta dinh thiet lap ket noi da duoc ket noi tu truoc chua
						if( hec_ID_will_connect == avoid_Repeating_HECs_ID[j] ){
							compare = 1;
							break;
						}											
					}

					if(compare == 1){				//neu lap roi thi thu mot ket noi khac
						i--;
						continue;
					}

					else{							//neu chua lặp thi update ket noi vua thiet lap
						//update HEC duoc ket noi vao database cua HEC moi duoc khoi tao 
						File.AppendAllText(file_Path, hec_ID_will_connect.ToString() + "\n");

						//update HEC moi khoi tao vao database cua MEC duoc ket noi
						string hec_will_connect_file_Path = "./database/MEC_" + mec_ID + "/HEC_" + hec_ID_will_connect.ToString() + ".txt";				

						string[] lines = File.ReadAllLines(hec_will_connect_file_Path);		//cap nhat lai tong so luong ket noi voi MEC khac - trong file cua MEC duoc MEC moi khoi tao ket noi den
						
						File.WriteAllText(hec_will_connect_file_Path, "");					//xoa toan bo file
						
						for( int m = 0 ; m < lines.Length ; m++){							//cap nhat lai
							if( lines[m] == "HEC_Connection" ){
								lines[m+1] = (Convert.ToInt32(lines[m+1]) + 1).ToString() ;
								File.AppendAllText(hec_will_connect_file_Path, lines[m] + "\n" + lines[m+1] + "\n");
								m++;
							}else{
								File.AppendAllText(hec_will_connect_file_Path, lines[m] + "\n");
							}
						}

						File.AppendAllText(hec_will_connect_file_Path, hec_ID + "\n");	//them MEC moi vao database duoc ket noi

						avoid_Repeating_HECs_ID[i-1] = hec_ID_will_connect;	
					}								
				}
				else{
					//update MEC duoc ket noi vao database cua MEC moi duoc khoi tao 
					File.AppendAllText(file_Path,  hec_ID_will_connect.ToString() + "\n");

					//update HEC moi khoi tao vao database cua MEC duoc ket noi
					string hec_will_connect_file_Path = "./database/MEC_" + mec_ID + "/HEC_" + hec_ID_will_connect.ToString() + ".txt";				

					string[] lines = File.ReadAllLines(hec_will_connect_file_Path);		//cap nhat lai tong so luong ket noi voi MEC khac - trong file cua MEC duoc MEC moi khoi tao ket noi den
					
					File.WriteAllText(hec_will_connect_file_Path, "");					//xoa toan bo file
					
					for( int m = 0 ; m < lines.Length ; m++){							//cap nhat lai
						if( lines[m] == "HEC_Connection" ){
							lines[m+1] = (Convert.ToInt32(lines[m+1]) + 1).ToString() ;
							File.AppendAllText(hec_will_connect_file_Path, lines[m] + "\n" + lines[m+1] + "\n");
							m++;
						}else{
							File.AppendAllText(hec_will_connect_file_Path, lines[m] + "\n");
						}
					}

					File.AppendAllText(hec_will_connect_file_Path, hec_ID + "\n");	//them MEC moi vao database duoc ket noi

					avoid_Repeating_HECs_ID[i-1] = hec_ID_will_connect;	
				}
			}
		}
	}
}