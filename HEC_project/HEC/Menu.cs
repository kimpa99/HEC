using System;

namespace HEC
{
    class Menu
    {
        //----------------------------------------------------Ham main_Menu------------------------------------------------------------------------------------------------------
        public void menu(){

            //bien de lam menu
            int i = 9, k = 0;
            char x = 'k';

            Nodes_Initial adding_Node = new Nodes_Initial();        //khoi tao object tu class nodes_Initial
            Nodes_Graph show_info = new Nodes_Graph();              //khoi tao object tu class Nodes_graph
            Simulation simulate = new Simulation();                 //khoi tao object tu class simulation
            Nodes_Remove remove = new Nodes_Remove();               //khoi tao object tu class Nodes_Remove


            Console.Clear();

            //hien thi menu
            Console.WriteLine("HEC simulation");
            Console.WriteLine("\n\nMenu");
            Console.WriteLine("1, Nodes initial \n2, Node removing \n3, Simulation \n4, Nodes graph \n5, Exit");

            while (k != 1) {
                x = Console.ReadKey().KeyChar;                      //input tuy chon (dinh dang char)
                Console.Clear();                                    //xoa man hinh hien thi

                //hien thi menu
                Console.WriteLine("HEC simulation");
                Console.WriteLine("\n\nMenu");
                Console.WriteLine("1, Nodes initial \n2, Node removing \n3, Simulation \n4, Nodes graph \n5, Exit");

                i = Convert.ToInt32(x);                             //convert tu char sang int


                if (i == '1' || i == '2' || i == '3' || i == '4' || i == '5')
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
                case '1': adding_Node.node_Adding();    break;      //chuyen sang phan node_Adding trong class Nodes_Initial
                case '2': remove.remove_menu();         break;
                case '3': simulate.simulation();        break;
                case '4': show_info.show_Nodes_info();  break;      //chuyen sang phan show_Nodes_info trong class Nodes_Graph
                case '5': break;
            }
        }
    
        //--------------------------------------------------Ham bat dau program------------------------------------------------------------------------------------------
        static void Main(string[] args)         
        {
            Menu main_Menu = new Menu();                            //khai class de su dung main_menu
            main_Menu.menu();                                       //chuyen sang main_menu
        }
    }
}
