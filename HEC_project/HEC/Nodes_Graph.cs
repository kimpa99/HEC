using System;
using System.IO;

namespace HEC{

	class Nodes_Graph{

		public void show_Nodes_info(){

			Console.Clear();
			
			string central_cloud_file_path = "./database/Central_Cloud.txt";	//Mo va doc toan bo file database Central_Cloud
			string[] lines = File.ReadAllLines(central_cloud_file_path);

			Console.WriteLine("\nCentral Cloud is initialized\n");				//In ra man hinh Central Cloud da duoc khoi tao

			for(int a = 0; a < lines.Length ; a++){								//Quet danh sach MEC

				if( lines[a] == "MEC_Connection" ){

					int total_number_of_MEC = Convert.ToInt32(lines[a+1]);		//Lay tong so luong MEC co trong he thong

					Nodes_Graph MEC = new Nodes_Graph();						//Chuyen sang ham hien thi thong tin MEC ra Console
					MEC.show_MEC_info(total_number_of_MEC);
				}
			}

			Console.WriteLine("\n\nPress '1' to go back main menu");

			char x='k';
        	int l=9, k = 0;

            Menu main_Menu = new Menu();

        	while(k != 1){
        		x = Console.ReadKey().KeyChar;    	//nhap (input) tuy chon tu ban phim

                Console.Clear();					//xoa man hinh hien thi
                Console.WriteLine("\n\nWe have done here!\n\nPress '1' to go back main menu");

                l = Convert.ToInt32(x);     		//chuyen input tu char sang int

                if (l == '1')   					//so sanh de chon cac lua chon trong menu
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
                    main_Menu.menu();       		//quay tro ve main_menu
                    break;    
        	}
		}


		public void show_MEC_info(int total_number_of_MEC){						//Ham hien thi thong tin MEC ra console

			int total_MEC = total_number_of_MEC;								//Tong so luong MEC co trong he thong

			for(int b = 1 ; b <= total_MEC ; b++ ){

				string MEC_ID = b.ToString();

				string mec_file_path = "./database/MEC_" + MEC_ID + "/MEC_" + MEC_ID + ".txt";	//File MEC dang lay thong tin

				Console.WriteLine("\n\n---------------------------MEC_" + MEC_ID +"---------------------------");	//In ra console MEC_ID
				Console.WriteLine("Status: Connected to Central Cloud!\n");

				string[] lines = File.ReadAllLines(mec_file_path);

				for(int c = 0 ; c < lines.Length ; c++ ){

					if( lines[c] == "MEC_Connection" ){

						Console.WriteLine("Neighbor MECs:");					//In ra thong tin cac Neighbor MECs

						int total_neighbor_MEC = Convert.ToInt32(lines[c+1]);

						for(int d = 1 ; d <= total_neighbor_MEC ; d++){

							Console.WriteLine("MEC_" + lines[c + 1 + d]);
						}

						break;
					}
				}

				for(int d = 0 ; d < lines.Length ; d++ ){

					if( lines[d] == "HEC_Connection" ){							//In ra thong tin cac HEC co trong MEC dang xet

						Console.WriteLine("\nHEC list:");

						int total_number_of_HEC = Convert.ToInt32(lines[d+1]);	//Lay tong so luong HEC co trong MEC dang xet

						Nodes_Graph HEC = new Nodes_Graph();					//Chuyen sang ham hien thi thong tin HEC ra Console			
						HEC.show_HEC_info(b, total_number_of_HEC);
						break;
					}
				}
			}
		}


		public void show_HEC_info(int MEC_ID, int total_number_of_HEC){

			int total_HEC = total_number_of_HEC;
			int mec_id = MEC_ID;

			for( int e = 1 ; e <= total_HEC ; e++ ){

				string HEC_ID = e.ToString();

				string hec_file_path = "./database/MEC_" + mec_id.ToString() + "/HEC_" + HEC_ID + ".txt";

				Console.WriteLine("\t\n--------HEC_" + HEC_ID + "--------");

				string[] lines = File.ReadAllLines(hec_file_path);

				for(int f = 0 ; f < lines.Length ; f++){
					if( lines[f] == "HEC_Connection"){

						Console.WriteLine("Neighbor HEC:");

						int total_neighbor_HEC = Convert.ToInt32(lines[f+1]);

						for(int g = 1; g <= total_neighbor_HEC ; g++){

							Console.WriteLine("HEC_" + lines[f + 1 + g]);
						}

						break;
					}
				}
			}	
		}
	}
}