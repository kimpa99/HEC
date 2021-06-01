using System;
using System.IO;

namespace HEC{

	class Simulation{

		//---------------------------------------------------Input: n data blocks + Require time-------------------------------------
		public void simulation(){

			//random_Generator r = new random_Generator();
			//int total_number_of_data_blocks = r.random_Number_inrange(50, 100);		//tong so luong task random

			Console.Clear();
			/*
			Console.WriteLine("Simulation\n");
			Console.WriteLine("MEC serving: ");											//thong tin dau vao: MEC, HEC, so luong task
			string MEC_serving = Console.ReadLine();
			Console.WriteLine("HEC serving: ");
			string HEC_serving = Console.ReadLine();

			Console.WriteLine("Number of task: ");
			int total_number_of_data_blocks = Convert.ToInt32(Console.ReadLine());

			Simulation program = new Simulation();										//khoi tao object tu class simulation

			program.HEC_node_case( MEC_serving , HEC_serving , total_number_of_data_blocks );
			*/
			string MEC_serving = "1";													//Tao tu dong cac phep thu
			string HEC_serving = "1";
			int total_number_of_data_blocks = 0;

			for(int i = 0; i <= 2000 ; i = i + 100){

				total_number_of_data_blocks = i;
				Simulation program = new Simulation();
				program.HEC_node_case( MEC_serving , HEC_serving , total_number_of_data_blocks );
				program.MEC_only_case( MEC_serving , total_number_of_data_blocks );
			}

			//----Quay tro ve main_Menu
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

		//-------------------------------------------------Ket qua-----------------------------------------------------------------
		public void result(double time_total, int total_number_of_data_blocks){

			double total_Time = time_total;
			Console.WriteLine("\n\nTotal HEC-clustering offload time with " + total_number_of_data_blocks + " data blocks : " + total_Time);

			//Xuat so luong offloading_data ra file
			string number_of_data_file_Path = "./result/number_of_data_blocks.txt";
            string write_number_of_data = total_number_of_data_blocks.ToString();
            File.AppendAllText(number_of_data_file_Path , write_number_of_data + "\n");

            //Xuat thoi gian offloading ra file
            string total_Time_file_Path = "./result/offloading_time.txt";
            string write_total_time = total_Time.ToString();
            File.AppendAllText(total_Time_file_Path , write_total_time + "\n");

		}

		public void result_MEC(double time_total, int total_number_of_data_blocks){

			double total_Time = time_total;
			Console.WriteLine("\nTotal MEC-clustering offload time with " + total_number_of_data_blocks + " data blocks : " + total_Time);

            //Xuat thoi gian offloading ra file
            string total_Time_file_Path = "./result/offloading_time_MEC.txt";
            string write_total_time = total_Time.ToString();
            File.AppendAllText(total_Time_file_Path , write_total_time + "\n");

		}

		//-------------------------------------------------Truong hop offload tai HEC Node---------------------------------------
		public void HEC_node_case(string mec , string hec , int total_blocks ){
			string MEC_serving = mec;
			string HEC_serving = hec;
			int total_number_of_data_blocks = total_blocks;

			string hec_file_path = "./database/MEC_" + MEC_serving + "/HEC_" + HEC_serving + ".txt";
			string[] lines = File.ReadAllLines(hec_file_path);

			Simulation program = new Simulation();

			if( total_number_of_data_blocks <= Convert.ToInt32(lines[0]) ){

				double processing_Time = Convert.ToDouble(total_number_of_data_blocks) / Convert.ToDouble(lines[0]);	//thoi gian xu ly (ms)

				program.result(processing_Time, total_number_of_data_blocks);

			} else if ( total_number_of_data_blocks > Convert.ToInt32(lines[0]) ){					//khong du resources - chuyen len HEC cluster

				program.HEC_cluster_case(MEC_serving , HEC_serving , total_number_of_data_blocks);

			}			
		}

		//-------------------------------------------------Truong hop day len cluster-----------------------------------------
		public void HEC_cluster_case(string mec, string hec, int total_blocks){
			string MEC_serving = mec;									//MEC_serving
			string HEC_serving = hec;									//HEC_serving
			int total_number_of_data_blocks = total_blocks;				//tong so luong task can offload
			int data_block_remain = total_number_of_data_blocks;		//so luong task can offload con lai

			int total_hec_in_cluster = 0;								//tong so hec trong cluster
			int total_direct_connect_hec_in_cluster = 0;				//tong so HEC ket noi truc tiep voi HEC_serving
			double total_Time = 0;										//tong thoi gian offload
			double hec_hec_delay_Time = 0.001;							//do tre truyen truc tiep tu HEC nay sang HEC khac cua mot data block (ms/data block)

			string mec_file_path = "./database/MEC_" + MEC_serving + "/MEC_" + MEC_serving + ".txt";
			string[] a = File.ReadAllLines(mec_file_path);
			int mec_CPU = Convert.ToInt32( a[0] );						//CPU MEC_serving

			for(int b = 0 ; b < a.Length ; b++ ){
				if( a[b] == "HEC_Connection" ){
					total_hec_in_cluster = Convert.ToInt32(a[b+1]);		//lay tong so HEC trong cluster trong file MEC_serving
					break;
				}
			}

			string hec_file_path = "./database/MEC_" + MEC_serving + "/HEC_" + HEC_serving + ".txt";
			string[] c = File.ReadAllLines(hec_file_path);

			for (int d = 0; d < c.Length ; d++){
				if( c[d] == "HEC_Connection"){
					total_direct_connect_hec_in_cluster = Convert.ToInt32(c[d+1]);		//lay tong so HEC ket noi truc tiep voi HEC_serving trong cluster tu file HEC_serving
					break;
				}
			}

			int[] direct_connect_hec = new int[total_direct_connect_hec_in_cluster];	//mang luu tru cac HEC ket noi truc tiep voi HEC_serving

			for (int d = 0; d < c.Length ; d++){
				if( c[d] == "HEC_Connection"){
					for(int g = 0 ; g < total_direct_connect_hec_in_cluster ; g++ ){	//cap nhat mang luu tru cac HEC ket noi truc tiep voi HEC_serving - ok
						direct_connect_hec[g] = Convert.ToInt32(c[d + g + 2]);
					}
					break;
				}
			}

			data_block_remain = total_number_of_data_blocks - Convert.ToInt32(c[0]);	//so task con lai sau khi xu ly boi HEC_serving - ok
			
			//----------------Xu ly boi cac HEC_in_cluster------------
			for(int e = 1; e <= total_hec_in_cluster ; e++){							//xet lan luot cac HEC trong cluster

				if(e == Convert.ToInt32(HEC_serving) ){									//neu gap HEC_serving thi bo qua
					continue;
				}

				string hec_in_cluster_file_path = "./database/MEC_" + MEC_serving + "/HEC_" + e.ToString() + ".txt";
				string[] f = File.ReadAllLines(hec_in_cluster_file_path);
				int hec_in_cluster_cpu = Convert.ToInt32(f[0]);							//lay CPU cua HEC dang xet - ok

				//------HEC ket noi truc tiep - HEC_serving -> HEC_in_cluster -> Offload -> HEC_serving
				int mark_as_direct = 0;													//danh dau HEC ket noi truc tiep voi HEC_serving (mark_as_direct = 1 khi la ket noi truc tiep)

				for( int h = 0 ; h < total_direct_connect_hec_in_cluster ; h++){		//check xem HEC co ket noi truc tiep voi HEC_serving khong

					if( e == direct_connect_hec[h] ){									//truong hop ket noi truc tiep voi HEC_serving

						if( data_block_remain - hec_in_cluster_cpu > 0 ){				//neu khong du tai nguyen de xu ly

							total_Time = total_Time + hec_hec_delay_Time * hec_in_cluster_cpu;

							data_block_remain = data_block_remain - hec_in_cluster_cpu;

						} else if( data_block_remain - hec_in_cluster_cpu <= 0 ){		//neu du tai nguyen de xu ly

							total_Time = total_Time + 1 + 2 * hec_hec_delay_Time * data_block_remain;

							data_block_remain = 0;

						}

						mark_as_direct = 1;												//danh dau HEC ket noi truc tiep voi HEC_serving

						break;															//thoat vong for kiem tra cac HEC ket noi truc tiep sau khi da gap HEC ket noi truc tiep					
					}
				}

				if(data_block_remain == 0){												//neu HEC ket noi truc tiep da xu ly xong thi thoat khoi vong for
					break;
				}

				if(mark_as_direct == 1){												//neu HEC ket noi truc tiep da duoc xet thi bo qua xet truong hop HEC khong truc tiep
					continue;
				}	

				//----------HEC ket noi khong truc tiep - HEC_serving -> MEC -> HEC_in_cluster -> Offload -> MEC -> HEC_serving
				if( data_block_remain - hec_in_cluster_cpu > 0 ){						//HEC khong ket noi truc tiep voi HEC_serving-------

					total_Time = total_Time + 2 * hec_hec_delay_Time * hec_in_cluster_cpu;

					data_block_remain = data_block_remain - hec_in_cluster_cpu;

				} else if( data_block_remain - hec_in_cluster_cpu <= 0 ){

					total_Time = total_Time + 1 + 4 * hec_hec_delay_Time * data_block_remain;		//1 la 1ms xu ly n data block phu thuoc vao CPU cua HEC_in_cluster
					
					data_block_remain = 0;

				}

				if(data_block_remain == 0){												//neu HEC ket noi khong truc tiep da xu ly xong thi thoat khoi vong for
					break;
				}
			}

			//-------------Day not len MEC_serving xu ly-------------------
			if(data_block_remain != 0){													//neu chua xu ly het thi day len MEC_serving xu ly

				if( data_block_remain - mec_CPU <= 0 ){									//MEC_serving co the xu ly duoc het
					
					total_Time = total_Time + 1 + 2 * hec_hec_delay_Time * data_block_remain ;

					data_block_remain = 0;

				} else if ( data_block_remain - mec_CPU > 0 ){							//MEC_serving khong xu ly duoc het

					total_Time = total_Time + hec_hec_delay_Time * mec_CPU  ;				

					data_block_remain = data_block_remain - mec_CPU;
				}

			}
			
			Simulation program = new Simulation();										//khoi tao object tu class simulation
			
			if(data_block_remain == 0 ){

				program.result(total_Time , total_number_of_data_blocks);												//giai quyet xong thi in ra tong thoi gian offload

			}else{

				program.HEC_neighbor_case(MEC_serving, HEC_serving, data_block_remain , total_Time , total_number_of_data_blocks );	//chua giai quyen xong thi day sang neighbor_HEC_cluster

			}

		}	

		//-------------------------------------------------Truong hop day sang cluster ben canh-------------------------------
		public void HEC_neighbor_case(string mec , string hec , int data_remain, double time_total , int total_number_of_data_blocks ){

			string MEC_serving = mec;
			string HEC_serving = hec;									
			int data_block_remain = data_remain;												//so luong data_block can offload con lai
			double total_Time = time_total;														//tong thoi gian offload (tinh ca thoi gian offload truoc)

			double hec_mec = 0.001;																//do tre truyen truc tiep tu HEC nay sang HEC khac (MEC_serving) cua mot data block (ms/data block)
			double mec_mec = 0.01;																//do tre truyen truc tiep tu MEC nay sang MEC khac cua mot data block (ms/data block)

			//Cap nhat tong so MEC tu file Cloud
			string cloud_file_path = "./database/Central_Cloud.txt";
			string[] lines = File.ReadAllLines(cloud_file_path);
			int total_MEC = 0;

			for(int c = 0 ; c < lines.Length ; c++ ){
				if( lines[c] == "MEC_Connection" ){
					total_MEC = Convert.ToInt32( lines[c+1] );									//tong so MEC trong he thong (lay tu file Central_cloud)			
				}
			}

			//Cap nhat tong so MEC_neighbor va luu vao mang (lay tu file MEC_serving)
			string mec_serving_file_path = "./database/MEC_" + MEC_serving + "/MEC_" + MEC_serving + ".txt";
			string[] mec_serving_data = File.ReadAllLines(mec_serving_file_path);
			int total_neighbor_MEC = 0;															//Tong so MEC_neighbor

			for(int b = 0 ; b < mec_serving_data.Length ; b++ ){
				if( mec_serving_data[b] == "MEC_Connection"){
					total_neighbor_MEC = Convert.ToInt32( mec_serving_data[b+1] );				//Cap nhat tong so MEC_neighbor tu file MEC_serving
					break;
				}
			}

			int[] neighbor_MEC_list = new int[total_neighbor_MEC];

			for(int d = 0 ; d < mec_serving_data.Length ; d++ ){
				if( mec_serving_data[d] == "MEC_Connection" ){
					for( int e = 0 ; e < total_neighbor_MEC ; e++ ){
						neighbor_MEC_list[e] = Convert.ToInt32( mec_serving_data[d + e + 2] );	//Cap nhat mang chua cac MEC_neighbor - ok
					}
					break;
				}
			}

			//--------------------Quet cac MEC trong he thong
			for( int i = 1 ; i <= total_MEC ; i++ ){

				if( i == Convert.ToInt32(MEC_serving) ){										//Neu la MEC_serving thi bo qua
					continue;
				}

				//Check va danh dau MEC_nighbor
				int mark_as_neighbor = 0;

				for( int j = 0 ; j < total_neighbor_MEC ; j++ ){
					if( i == neighbor_MEC_list[j] ){
						mark_as_neighbor = 1;							
						break;
					}
				}

				//-----------------Offload Task boi MEC_neighbor
				if( mark_as_neighbor == 1 ){													//Truong hop la MEC_neighbor

					string mec_neighbor_file_path = "./database/MEC_" + i.ToString() + "/MEC_" + i.ToString() + ".txt";
					string[] mec_neighbor_data = File.ReadAllLines(mec_neighbor_file_path);
					int mec_neighbor_CPU = Convert.ToInt32( mec_neighbor_data[0] );				//CPU cua MEC_neighbor

					if( data_block_remain - mec_neighbor_CPU > 0 ){								//Truong hop khong xu ly het data_remain

						total_Time = total_Time + (hec_mec + mec_mec) * mec_neighbor_CPU ;

						data_block_remain = data_block_remain - mec_neighbor_CPU;

					} else if( data_block_remain - mec_neighbor_CPU <= 0 ){						//Truong hop xu ly het data_remain

						total_Time = total_Time + 1 + 2 * (hec_mec + mec_mec) * data_block_remain;

						data_block_remain = 0;

					}

					if( data_block_remain == 0 ){												//Neu MEC_neighbor da xu ly xong thi khong can cac HECs_nighbor giai quyet nua -> break vong for

						break;

					} else {																	//Neu MEC_neighbor chua xy ly xong thi day task vao cac HEC_neighbor

						//-----------------------Offload Task voi HEC_neighbor
						int total_neighbor_HEC = 0;												//Lay tong so luong HEC_neighbor

						for( int k = 0 ; k < mec_neighbor_data.Length ; k++ ){
							if( mec_neighbor_data[k] == "HEC_Connection" ){
								total_neighbor_HEC = Convert.ToInt32( mec_neighbor_data[k+1] );	//Tong so HEC trong MEC_neighbor
								break;
							}
						}

						for( int l = 1 ; l <= total_neighbor_HEC ; l++){						//day task lan luot vao cac HEC_neighbor
							
							string hec_neighbor_file_path = "./database/MEC_" + i.ToString() + "/HEC_" + l.ToString() + ".txt";
							string[] hec_neighbor_data = File.ReadAllLines(hec_neighbor_file_path);
							int hec_neighbor_CPU = Convert.ToInt32( hec_neighbor_data[0] );		//Lay HEC_neighbor_CPU

							if( data_block_remain - hec_neighbor_CPU > 0 ){						//Truong hop khong xu ly het data_remain

								total_Time = total_Time + (hec_mec + mec_mec + hec_mec) * hec_neighbor_CPU ;

								data_block_remain = data_block_remain - hec_neighbor_CPU;

							} else if( data_block_remain - hec_neighbor_CPU <= 0 ){				//Truong hop xu ly het data_remain

								total_Time = total_Time + 1 + 2 * (hec_mec + mec_mec + hec_mec) * data_block_remain ;

								data_block_remain = 0;

								break;

							}

						}

						if(data_block_remain == 0 ){
							break;
						}
					}

				} else {																		//Khong phai la MEC_neighbor thi bo qua
					
					continue;

				}
			}

			Simulation program = new Simulation();										//khoi tao object tu class simulation
			
			if(data_block_remain == 0 ){

				program.result(total_Time, total_number_of_data_blocks);												//giai quyet xong thi in ra tong thoi gian offload

			}else{

				program.Cloud_case(data_block_remain , total_Time , total_number_of_data_blocks );

			}

		}

		//-------------------------------------------------Truong hop day len cloud-------------------------------------------
		public void Cloud_case(int data_remain , double time_total , int total_number_of_data_blocks){

			int data_block_remain = data_remain;
			double total_Time = time_total;

			double hec_mec = 0.001;
			double mec_cloud = 0.1;

			string cloud_file_path = "./database/Central_Cloud.txt";
			string[] lines = File.ReadAllLines(cloud_file_path);
			int cloud_CPU = Convert.ToInt32( lines[0] );					//Cloud_CPU

			if( data_block_remain - cloud_CPU > 0 ){

				total_Time = total_Time + ( Convert.ToDouble(data_block_remain) / Convert.ToDouble(cloud_CPU) ) + 2 * (hec_mec + mec_cloud) * data_block_remain ;

			}else if( data_block_remain - cloud_CPU <= 0 ){

				total_Time = total_Time + 1 + 2 * (hec_mec + mec_cloud) * data_block_remain;

			}	

			data_block_remain = 0;

			Simulation program = new Simulation();							//khoi tao object tu class simulation
			program.result(total_Time, total_number_of_data_blocks);
			
		}

		//-------------------------------------------------Mo phong trong mo hinh chi co MEC (dung de so sanh voi do tre cua HEC)----------------------------------
		public void MEC_only_case(string mec , int data_block){

			string MEC_serving = mec;
			int total_number_of_data_blocks = data_block;

			int data_block_remain = total_number_of_data_blocks;
			double total_Time = 0;

			double mec_mec = 0.01;
			double mec_cloud = 0.1;

			//Cap nhat tong so MEC tu file Cloud
			string cloud_file_path = "./database/Central_Cloud.txt";
			string[] cloud_data = File.ReadAllLines(cloud_file_path);
			int total_MEC = 0;

			for(int a = 0 ; a < cloud_data.Length ; a++ ){
				if( cloud_data[a] == "MEC_Connection" ){
					total_MEC = Convert.ToInt32( cloud_data[a+1] );								//tong so MEC trong he thong (lay tu file Central_cloud)			
				}
			}

			//Cap nhat tong so MEC_neighbor va luu vao mang (lay tu file MEC_serving)
			string mec_serving_file_path = "./database/MEC_" + MEC_serving + "/MEC_" + MEC_serving + ".txt";
			string[] mec_serving_data = File.ReadAllLines(mec_serving_file_path);
			int total_neighbor_MEC = 0;															//Tong so MEC_neighbor

			for(int b = 0 ; b < mec_serving_data.Length ; b++ ){
				if( mec_serving_data[b] == "MEC_Connection"){
					total_neighbor_MEC = Convert.ToInt32( mec_serving_data[b+1] );				//Cap nhat tong so MEC_neighbor tu file MEC_serving
					break;
				}
			}

			int[] neighbor_MEC_list = new int[total_neighbor_MEC];

			for(int c = 0 ; c < mec_serving_data.Length ; c++ ){
				if( mec_serving_data[c] == "MEC_Connection" ){
					for( int d = 0 ; d < total_neighbor_MEC ; d++ ){
						neighbor_MEC_list[d] = Convert.ToInt32( mec_serving_data[c + d + 2] );	//Cap nhat mang chua cac MEC_neighbor
					}
					break;
				}
			}

			Simulation program = new Simulation();											//khoi tao object tu class simulation


			int mec_serving_CPU = Convert.ToInt32( mec_serving_data[0] );						//mec_serving_CPU

			//---------------------------------------------neu MEC_serving_CPU xu ly duoc het---------------------------------------------
			if( total_number_of_data_blocks - mec_serving_CPU <= 0 ){							

				total_Time = Convert.ToDouble(total_number_of_data_blocks) / Convert.ToDouble(mec_serving_CPU) ;

				data_block_remain = 0;

				program.result_MEC(total_Time, total_number_of_data_blocks);

			//------------------------------neu MEC_serving_CPU khong xu ly duoc het thi day sang cac MEC_neighbor-----------------------
			} else if ( total_number_of_data_blocks - mec_serving_CPU > 0 ) {					

				data_block_remain = total_number_of_data_blocks - mec_serving_CPU;					//data con lai sau khi duoc xu ly moi MEC_serving

				for( int i = 1 ; i <= total_MEC ; i++ ){											//quet tat ca MEC trong he thong

					if( i == Convert.ToInt32(MEC_serving) ){										//Neu la MEC_serving thi bo qua
						continue;
					}

					//Check va danh dau MEC_nighbor
					int mark_as_neighbor = 0;

					for( int j = 0 ; j < total_neighbor_MEC ; j++ ){
						if( i == neighbor_MEC_list[j] ){
							mark_as_neighbor = 1;							
							break;
						}
					}

					//-----------------Offload Task boi MEC_neighbor
					if( mark_as_neighbor == 1 ){													//Truong hop la MEC_neighbor

						string mec_neighbor_file_path = "./database/MEC_" + i.ToString() + "/MEC_" + i.ToString() + ".txt";
						string[] mec_neighbor_data = File.ReadAllLines(mec_neighbor_file_path);
						int mec_neighbor_CPU = Convert.ToInt32( mec_neighbor_data[0] );				//CPU cua MEC_neighbor

						if( data_block_remain - mec_neighbor_CPU > 0 ){								//Truong hop khong xu ly het data_remain

							total_Time = total_Time + mec_mec * mec_neighbor_CPU ;

							data_block_remain = data_block_remain - mec_neighbor_CPU;

						} else if( data_block_remain - mec_neighbor_CPU <= 0 ){						//Truong hop xu ly het data_remain

							total_Time = total_Time + 1 + 2 * mec_mec * data_block_remain;

							data_block_remain = 0;

						}

						if( data_block_remain == 0 ){												//Neu MEC_neighbor da xu ly xong thi khong can cac HECs_nighbor giai quyet nua -> break vong for

							break;

						}

					}else{																			//Khong phai la MEC_neighbor thi bo qua
						
						continue;

					}
				}

				int cloud_CPU = Convert.ToInt32( cloud_data[0] );

				if( data_block_remain != 0 ){														//Neu cac MEC chua giai quyet het data thi day not so con lai len cloud

					if(data_block_remain - cloud_CPU > 0){

						total_Time = total_Time + 2 * Convert.ToDouble(data_block_remain) * Convert.ToDouble(mec_cloud) + Convert.ToDouble(data_block_remain) / Convert.ToDouble(cloud_CPU) ;

					} else if (data_block_remain - cloud_CPU <= 0) {
						
						total_Time = total_Time + 1 + 2 * Convert.ToDouble(data_block_remain) * Convert.ToDouble(mec_cloud) ;

					}

					data_block_remain = 0;
				}

				program.result_MEC(total_Time, total_number_of_data_blocks);
			}
		}
	}
}