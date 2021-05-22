using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oracle_PhoneBook
{
    class Program
    {
        static void Main(string[] args)
        {
            string strConn = "Data Source=(DESCRIPTION=" +
                "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
                "(HOST=localhost)(PORT=1521)))" +
                "(CONNECT_DATA=(SERVER=DEDICATED)" +
                "(SERVICE_NAME=xe)));" +
                "User Id=hr;Password=hr;";

            OracleConnection conn = new OracleConnection(strConn);

            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;


            int a = 1;
            do
            {
                Console.WriteLine("1. 주소록 테이블 생성");
                Console.WriteLine("2. 주소록 조회");
                Console.WriteLine("3. 주소록 추가");
                Console.WriteLine("4. 주소록 전화번호 수정");
                Console.WriteLine("5. 주소록 삭제");
                Console.WriteLine("6. 주소록 테이블 삭제");
                Console.WriteLine("7. 나가기");

                Console.WriteLine();
                Console.Write("선택할 메뉴 번호를 입력하세요 : ");
                string n = Console.ReadLine();

                Console.WriteLine();
                Console.WriteLine("----------------------------------");
                Console.WriteLine();

                switch (n)
                {
                    case "1":
                        try
                        {
                            cmd.CommandText = "create table phonebook" +
                            "(ID number(4) primary key," +
                            "Name varchar(20)," +
                            "HP varchar(20))";
                            cmd.ExecuteNonQuery();
                            // 이미 테이블 존재하면 예외처리
                            cmd.CommandText = "create sequence seq ";
                            cmd.ExecuteNonQuery();
                            // 테이블 생성하면서 시퀀스 생성
                            Console.WriteLine("테이블이 생성되었습니다.");

                        }
                        catch (Exception)
                        {
                            Console.WriteLine("             #### 오류 ####");
                            Console.WriteLine();
                            Console.WriteLine("주소록이 이미 생성되었거나, 시퀀스가 존재합니다");
                        }
                        break;
                    case "2":
                        try
                        {
                            Console.WriteLine("1. ㄱ ~ ㅎ 이름 순서로 조회");
                            Console.WriteLine("2. 추가한 순서별로 조회");
                            Console.WriteLine();

                            Console.Write("선택할 메뉴 번호를 입력하세요 : ");
                            int k = int.Parse(Console.ReadLine());
                            Console.WriteLine();
                            Console.WriteLine("----------------------------------");
                            Console.WriteLine();

                            if (k == 1)
                            {
                                cmd.CommandText = "select * from Phonebook order by name";
                                cmd.ExecuteNonQuery();
                            }
                            if (k == 2)
                            {
                                cmd.CommandText = "select * from Phonebook order by id";
                                cmd.ExecuteNonQuery();
                            }

                            OracleDataReader rdr = cmd.ExecuteReader(); // 데이터 조회 결과 리턴하는 객체
                            while (rdr.Read())
                            {
                                int id = rdr.GetInt32(0);
                                string Name = rdr["Name"] as string;
                                string HP = rdr["HP"] as string;

                                Console.WriteLine($"{id} : {Name} : {HP}");
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("             #### 오류 ####");
                            Console.WriteLine();
                            Console.WriteLine("주소록이 생성되지 않았습니다. 주소록 테이블을 먼저 생성해주세요.");
                        }
                        break;

                    case "3":
                        bool yesORno = true;
                        do
                        {
                            try
                            {
                                Console.Write("이름을 입력하세요 : ");
                                string name = Console.ReadLine();

                                Console.Write("폰 번호를 입력하세요 : ");
                                string phonenum = Console.ReadLine();

                                cmd.CommandText = "insert into phonebook " +
                                    $"values(seq.nextval, '{name}', '{phonenum}')";
                                cmd.ExecuteNonQuery();

                                Console.WriteLine();
                                Console.WriteLine($"'{name}'를(을) 주소록에 추가했습니다.");
                                Console.WriteLine();
                                Console.Write("계속해서 주소록을 추가하시겠습니까? ( Y / N ) ->  ");
                                string YorN = Console.ReadLine();
                                if (YorN.ToUpper() != "Y" || YorN.ToLower() != "y")
                                {
                                    if (YorN.ToUpper() == "N" || YorN.ToLower() == "n")
                                    {
                                        yesORno = false;
                                        Console.WriteLine();
                                        Console.WriteLine("메뉴로 돌아갑니다..");
                                    }
                                    else
                                    {
                                        yesORno = false;

                                        Console.WriteLine();
                                        Console.WriteLine("----------------------------------");
                                        Console.WriteLine("********");
                                        Console.WriteLine("오류!!!!");
                                        Console.WriteLine("********");
                                        Console.WriteLine();
                                        Console.WriteLine("Y 또는 N를 입력하지 않았습니다.");
                                        Console.WriteLine("메뉴로 돌아갑니다.");
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                Console.WriteLine();
                                Console.WriteLine("             #### 오류 ####");
                                Console.WriteLine();
                                Console.WriteLine("주소록이 생성되지 않았습니다. 주소록 테이블을 먼저 생성해주세요.");
                                yesORno = false;
                            }
                        }
                        while (yesORno == true);

                        break;

                    case "4":
                        try
                        {
                            Console.Write("수정할 번호의 이름을 입력하세요 : ");
                            string modify_name = Console.ReadLine();

                            Console.Write("수정할 번호를 입력하세요 : ");
                            string modify_num = Console.ReadLine();

                            cmd.CommandText = "update phonebook " +
                                $"set hp = '{modify_num}' where name = '{modify_name}'";
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("변경이 완료되었습니다.");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine();
                            Console.WriteLine("             #### 오류 ####");
                            Console.WriteLine();
                            Console.WriteLine("주소록이 생성되지 않았습니다. 주소록 테이블을 먼저 생성해주세요.");
                        }
                        break;

                    case "5":
                        try
                        {
                            Console.Write("삭제할 번호의 이름을 입력하세요 : ");
                            string del_name = Console.ReadLine();
                            cmd.CommandText = "delete phonebook where name = " +
                                $"'{del_name}'";
                            cmd.ExecuteNonQuery();
                            Console.WriteLine();
                            Console.WriteLine($"'{del_name}'의 정보가 삭제되었습니다.");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine();
                            Console.WriteLine("             #### 오류 ####");
                            Console.WriteLine();
                            Console.WriteLine("주소록이 생성되지 않았습니다. 주소록 테이블을 먼저 생성해주세요.");
                        }
                        break;

                    case "6":
                        try
                        {
                            cmd.CommandText = "drop sequence seq";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "drop table Phonebook";
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("주소록 테이블과 시퀀스가 삭제되었습니다.");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("             #### 오류 ####");
                            Console.WriteLine();
                            Console.WriteLine("주소록이 이미 삭제되었거나, 시퀀스가 존재하지 않습니다.");
                        }
                        // 이미 테이블 지워졌으면 예외처리
                        // 또는 이미 시퀀스 지워졌으면 예외처리
                        break;
                    case "7":
                        a = 0;
                        conn.Close();
                        Console.WriteLine("종료..........");
                        break;
                    default:
                        Console.WriteLine("메뉴 번호 이외의 문자를 입력했습니다.");
                        Console.WriteLine("정확한 메뉴 번호를 입력해주세요");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("----------------------------------");
                Console.WriteLine();
            }
            while (a > 0);

            conn.Close();
        }
    }
}
