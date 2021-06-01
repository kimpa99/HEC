using System;
using System.IO;

namespace HEC{

	class Nodes_Remove{
//-------------------------------------------------Removing menu-------------------------------------------
		public void remove_menu(){

			Nodes_Remove remove = new Nodes_Remove();
			Menu main_Menu = new Menu();

			Console.Clear();
            Console.WriteLine("HEC simulation");
            Console.WriteLine("\n\nNode removing menu");
            Console.WriteLine("What do you want to remove, MEC or HEC? \n1, MEC \n2, HEC");

            int i = 9, k = 0;
            char x = 'k';

            while (k != 1) {
                x = Console.ReadKey().KeyChar;      		//input tuy chon (dinh dang char)
                Console.Clear();                    		//xoa man hinh hien thi

                //hien thi menu
	            Console.WriteLine("HEC simulation");
	            Console.WriteLine("\n\nNode removing menu");
	            Console.WriteLine("What do you want to remove, MEC or HEC? \n1, MEC \n2, HEC");

                i = Convert.ToInt32(x);             		//convert tu char sang int


                if (i == '1' || i == '2' || i == '3')
                {
                    k = 1;
                }
                else
                {
                    k = 0;
                }
            }

            //lua chon tuy chon trong menu
            switch (i) {
                case '1': remove.remove_MEC();	break;		//chuyen sang phan remove MEC
                case '2': remove.remove_HEC();	break;		//chuyen sang phan remove HEC
                case '3': main_Menu.menu(); 	break;		//chuyen sang phan main menu
            }
		}

        //---------------------------------------Quay tro ve menu khi xong
        public void end_remove(){

            Console.WriteLine("\n\nWe have done here!\n\nPress '1' to go back main menu");

            char x='k';
            int l=9, k = 0;

            Menu main_Menu = new Menu();

            while(k != 1){
                x = Console.ReadKey().KeyChar;                              //nhap (input) tuy chon tu ban phim

                Console.Clear();                                            //xoa man hinh hien thi
                Console.WriteLine("\n\nWe have done here!\n\nPress '1' to go back main menu");

                l = Convert.ToInt32(x);                                     //chuyen input tu char sang int

                if (l == '1')                                               //so sanh de chon cac lua chon trong menu
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
                    main_Menu.menu();                                       //quay tro ve main_menu
                    break;    
            }
        }

//-----------------------------------------------Removing MEC---------------------------------------------
		public void remove_MEC(){
			Console.Clear();
          	Console.WriteLine("HEC simulation");
          	Console.WriteLine("\n\nNode removing menu");
          	Console.WriteLine("Which MEC ID do you want to remove?");

          	int MEC_ID = Convert.ToInt32(Console.ReadLine());                                  

          	string remove_mec_directory_path = "./database/MEC_" + MEC_ID.ToString();			 //duong dan thu muc cua MEC can remove

            Nodes_Remove done = new Nodes_Remove();

            if(!Directory.Exists(remove_mec_directory_path)){                                       //kiem tra xem MEC_ID muon xoa co ton tai khong

                Console.Clear();

                Console.WriteLine("Somethings went wrong!!!");

                done.end_remove();                                                                 //khong ton tai thi bao loi
            }

            else{                                                                                   //remove_MEC_ID co ton tai thi xu ly

              	DirectoryInfo directory = new DirectoryInfo(remove_mec_directory_path);
              	directory.Delete(true);                                                              //xoa thu muc MEC can remove

              	//------Update Central_Cloud------------
              	string cloud_file_path = "./database/Central_Cloud.txt";           					 //duong dan file central cloud
              	string[] cloud_data = File.ReadAllLines(cloud_file_path);                            //luu tat ca du lieu file Cloud ra mang
              	int total_number_MEC = 0;

              	for(int i = 0 ; i < cloud_data.Length ; i++){

                  	if( cloud_data[i] == "MEC_Connection"){

                    	total_number_MEC = Convert.ToInt32(cloud_data[i+1]);                          //Lay tong so MEC tu file Central_Cloud

                    	cloud_data[i+1] = (Convert.ToInt32(cloud_data[i+1]) - 1).ToString() ;     	  //cap nhat lai tong so MEC

                    	break;

                  	} 

              	}

              	File.WriteAllText(cloud_file_path, "");            									//xoa toan bo noi dung file Central_Cloud
              	
              	for(int i = 0 ; i < cloud_data.Length ; i++){              							//cap nhat lai toan bo file Central_Cloud

              		if( i > 3 && cloud_data[i] != "\n" ){

              			if( Convert.ToInt32( cloud_data[i] ) == MEC_ID ){

              				continue;

              			}

              			if( Convert.ToInt32( cloud_data[i] ) > MEC_ID ){

              				File.AppendAllText(cloud_file_path, (Convert.ToInt32(cloud_data[i]) - 1).ToString()  + "\n");
              			
              			}

              			else{

              				File.AppendAllText(cloud_file_path, cloud_data[i] + "\n");
              				
              			}
                        
              		}

              		else{

              			File.AppendAllText(cloud_file_path, cloud_data[i] + "\n");

              		}
                
              	}

              	//------Update MEC that connect with removing_MEC-------
              	for( int i = 1; i <= total_number_MEC ; i++){										//Quet tung MEC co trong he thong

                    if( i == MEC_ID){                                                  				//Neu la remove_MEC thi bo qua

                       	continue;

                    }

                    string mec_file_path = "./database/MEC_" + i.ToString() + "/MEC_" + i.ToString() + ".txt";      //file path
                    string mec_directory_path = "./database/MEC_" + i.ToString();                                   //directory path

                    string new_mec_file_path = "./database/MEC_" + i.ToString() + "/MEC_" + (i - 1).ToString() + ".txt";    //new file path
                    string new_mec_directory_path = "./database/MEC_" + (i - 1).ToString();                                 //new directory path

                    int total_number_neighbor_mec = 0;
                    int mark_as_neighbor = 0;

                    string[] mec_data = File.ReadAllLines(mec_file_path);                			//luu du lieu trong file MEC dang xet ra mang

                    for(int j = 0 ; j < mec_data.Length ; j++){                                		//cap nhat lai tong so mec_neighbor
                          
                      	if(  mec_data[j] == "MEC_Connection" ){
                                
                            total_number_neighbor_mec = Convert.ToInt32( mec_data[ j + 1 ] );

                            for(int k = 0 ; k < total_number_neighbor_mec ; k++){
                                      
                              	if( mec_data[ j + 2 + k] == MEC_ID.ToString() ){                  	//neu trong mec_neighbor_list co MEC_ID cua remove_MEC <=> Day la MEC can cap nhat thong tin
                                            
                                    mec_data[ j + 1 ] = (Convert.ToInt32( mec_data[ j + 1 ] ) - 1 ).ToString(); 		//Cap nhat lai tong so MEC (-1 MEC remove)
                                            
                                    mark_as_neighbor = 1;                                           //danh dau day la MEC_neighbor
                                            
                                    break;
                                      
                              	}
                                
                            }

                            break;

                      	}

                    }

                    int mark = 0;                                                                   //marker doan dong viet file

                    if( mark_as_neighbor == 1){                                 					//neu MEC dang xet la MEC_neighbor

                      	File.WriteAllText(mec_file_path, "");                      					//xoa toan bo data trong file

                      	for(int j = 0 ; j < mec_data.Length ; j++){                 				//Ghi file den phan MEC_Connection

                            if( mec_data[j] == "MEC_Connection" ){

                                File.AppendAllText(mec_file_path, mec_data[j] + "\n");

                              	File.AppendAllText(mec_file_path, mec_data[j+1] + "\n");

                              	mark = j + 2;

                              	break;

                            }else{

                              	File.AppendAllText(mec_file_path, mec_data[j] + "\n");

                            }
                      	}

                      	for(int j = mark ; j < mec_data.Length ; j++){                               //viet file phan MEC_ID

                            if( mec_data[j] == "HEC_Connection" ){

                              	mark = j;

                              	break;

                            }else if ( mec_data[j] == MEC_ID.ToString() ) {
                                            
                               	continue;

                            }else{ 
                                      
                              	int check_mec = Convert.ToInt32( mec_data[ j ] );

                              	if( check_mec > MEC_ID){

                                    mec_data[j] = (check_mec - 1).ToString();

                                    File.AppendAllText(mec_file_path, mec_data[j] + "\n");

                              	}else{

                                	File.AppendAllText(mec_file_path, mec_data[j] + "\n");

                              	}
                            }
                      	}

                      	for(int j = mark ; j < mec_data.Length ; j++){                               //viet file tu doan HEC do di

                        	File.AppendAllText(mec_file_path, mec_data[j] + "\n");

                      	}
                    }

                    if( i > MEC_ID ){														         //doi ten file va doi ten thu muc

                      	File.Move( mec_file_path , new_mec_file_path );                              //doi ten file

                      	Directory.Move( mec_directory_path , new_mec_directory_path );               //doi ten thu muc

                   	}
            	}
                done.end_remove();
            }
       	}

//----------------------------------------------Removing HEC----------------------------------------------
		public void remove_HEC(){
			Console.Clear();
            Console.WriteLine("HEC simulation");
            Console.WriteLine("\n\nNode removing menu");

            Console.WriteLine("MEC contains removing HEC:");
            int MEC_ID = Convert.ToInt32(Console.ReadLine());                                    

            Console.WriteLine("HEC you want to remove:");
            int HEC_ID = Convert.ToInt32(Console.ReadLine());

            string mec_file_path = "./database/MEC_" + MEC_ID.ToString() + "/MEC_" + MEC_ID.ToString() + ".txt";        //mec_remove_file_path
            string hec_file_path = "./database/MEC_" + MEC_ID.ToString() + "/HEC_" + HEC_ID.ToString() + ".txt";        //hec_remove_file_path

            Nodes_Remove done = new Nodes_Remove();

            if(!File.Exists(hec_file_path)){

                Console.Clear();                                     

                Console.WriteLine("Wrong");

                done.end_remove();                                                                 //khong ton tai thi bao loi

            } else {

                File.Delete(hec_file_path);

                int total_number_HEC = 0;

                string[] mec_data = File.ReadAllLines(mec_file_path);

                for(int i = 0 ; i < mec_data.Length ; i++){

                    if( mec_data[i] == "HEC_Connection" ){

                        total_number_HEC = Convert.ToInt32( mec_data[i+1] );                        //tong so HEC

                        mec_data[i+1] = (Convert.ToInt32(mec_data[i+1]) - 1).ToString();            //cap nhat lai tong so HEC trong MEC

                        break;
                    }
                }

                File.WriteAllText(mec_file_path , "");                                              //xoa toan bo file MEC

                int mark_1 = 0;                                                                     //danh dau

                //---------------------Update file MEC_revome (MEC chua HEC remove)--------------
                for(int i = 0 ; i < mec_data.Length ; i++ ){                                        

                    if( mec_data[i] == "HEC_Connection" ){

                        mark_1 = i;

                        File.AppendAllText(mec_file_path , mec_data[i] + "\n" );                                   //ghi file den HEC_Connection

                        break;

                    } else {

                        File.AppendAllText(mec_file_path , mec_data[i] + "\n" );

                    }
                }

                for(int i = mark_1 + 1 ; i < mec_data.Length ; i++){

                    if( i > mark_1 + 1 && mec_data[i] != "\n" ){

                        if( Convert.ToInt32(mec_data[i]) == HEC_ID ){

                            continue;

                        } else if ( Convert.ToInt32(mec_data[i]) > HEC_ID ){

                            File.AppendAllText(mec_file_path , (Convert.ToInt32(mec_data[i]) - 1).ToString() + "\n" );     //ghi file tu total_hec den het
                            
                        } else {

                            File.AppendAllText(mec_file_path , mec_data[i] + "\n" );
                        }

                    } else {

                        File.AppendAllText(mec_file_path , mec_data[i] + "\n" );
                    }
                }

                //-------------------Update HEC files--------------------------------------
                for( int i = 1 ; i <= total_number_HEC ; i++ ){

                    if( i == HEC_ID ){

                        continue;
                    }

                    string hec_checking_file_path = "./database/MEC_" + MEC_ID.ToString() + "/HEC_" + i.ToString() + ".txt";
                    string new_hec_checking_file_path = "./database/MEC_" + MEC_ID.ToString() + "/HEC_" + (i - 1).ToString() + ".txt";

                    int total_hec_neighbor = 0;

                    int mark_as_neighbor = 0;

                    string[] hec_data = File.ReadAllLines(hec_checking_file_path);

                    for(int j = 0 ; j < hec_data.Length ; j++ ){

                        if( hec_data[j] == "HEC_Connection" ){

                            total_hec_neighbor = Convert.ToInt32( hec_data[j + 1] );

                            for( int k = 0 ; k < total_hec_neighbor ; k++ ){

                                if( Convert.ToInt32(hec_data[j + 2 + k]) == HEC_ID ){

                                    hec_data[j+1] = (Convert.ToInt32(hec_data[j+1]) - 1).ToString();

                                    mark_as_neighbor = 1;

                                    break;
                                }
                            }

                            break;
                        }
                    }

                    if( mark_as_neighbor == 1 ){

                        File.WriteAllText(hec_checking_file_path , "");

                        for( int j = 0 ; j < hec_data.Length ; j++ ){

                            if( j > 3 && hec_data[j] != "\n"){

                                if( Convert.ToInt32( hec_data[j] ) == HEC_ID ){

                                    continue;

                                } else if( Convert.ToInt32( hec_data[j] ) > HEC_ID ){

                                    File.AppendAllText(hec_checking_file_path , (Convert.ToInt32(hec_data[j]) - 1).ToString() + "\n" );

                                } else {

                                    File.AppendAllText(hec_checking_file_path , hec_data[j] + "\n" );

                                }

                            } else {

                                File.AppendAllText(hec_checking_file_path , hec_data[j] + "\n" );

                            }
                        }
                    }

                    if( i > HEC_ID ){

                        File.Move(hec_checking_file_path , new_hec_checking_file_path );

                    }
                }

                done.end_remove();
            }
		}
	}
}