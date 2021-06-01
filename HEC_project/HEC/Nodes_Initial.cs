using System;
using System.IO;

namespace HEC{

      //--------------------------------------------------------------------------------Ham khoi tao cac nodes Cloud - MECs - HECs-------------------------------------------------------------------------------------
	class Nodes_Initial{

		public void node_Adding(){

            Console.Clear();

            Console.WriteLine("\n\nInitializing Central Cloud...\n\n");           //thong bao tao xong Cloud

            //khoi tao central_cloud
            Central_Cloud central = new Central_Cloud();        //khoi tao object Central_Cloud
            central.initial();                                  //ham khoi tao trong class Central_Cloud

            Console.WriteLine("\n\nCentral Cloud initial done...\n\n");           //thong bao tao xong MECs

            //khoi tao MECs va HECs
            MEC create_MEC = new MEC();                         //khoi tao object MEC
            create_MEC.initial();                               //ham khoi tao trong class MEC
		}
	}
}