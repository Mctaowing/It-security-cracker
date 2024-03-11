using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PasswordCrackerMaster2004
{
    internal class PasswordCrackerMaster
    {
        //create a BlockingCollection
        BlockingCollection<List<string>> chunks = new BlockingCollection<List<string>>();
        List<UserInfo> userInfos = new List<UserInfo>();

        public PasswordCrackerMaster()
        {
            CreateChunks("webster-dictionary.txt");
            userInfos = PasswordFileHandler.ReadPasswordFile("passwords.txt");
        }

        public void CreateChunks(string fileName)
        {
            int count = 0;

            List<string> words = new List<string>();

            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))

            using (StreamReader dictionary = new StreamReader(fs))
            {
                while (!dictionary.EndOfStream)
                {
                    String dictionaryEntry = dictionary.ReadLine();
                    count++;
                    if (count % 10000 != 0)
                    {
                        words.Add(dictionaryEntry);
                    }
                    else
                    {
                        chunks.Add(words);
                        words = new List<string>();
                    }
                }
                chunks.Add(words);
            }
        }
        public void Listen(int port) {

            TcpListener server = new TcpListener(port);
            server.Start();

            Console.WriteLine("server is listning");
            //accepts a cient when a connection request is sent
            TcpClient clientSocket = server.AcceptTcpClient();

            Console.WriteLine("slave connected");

            Stream stream = clientSocket.GetStream();

            StreamReader sr = new StreamReader(stream);
            StreamWriter sw = new StreamWriter(stream);

            sw.AutoFlush = true;
            //Reads a request from the client/slave
            string request = sr.ReadLine();
            //prins the reauest
           // Console.WriteLine(request);
           while(request != "bye")
            {
                if (request == "password")
                {
                    //send passwords to the slave
                    string passwords = JsonSerializer.Serialize(userInfos);
                    sw.WriteLine(passwords);

                }
                else if (request == "chunk")
                {
                    //send dictionary chunk to the slave
                    // sw.WriteLine("the first chunk from the blocking collection");
                    string chunk = JsonSerializer.Serialize(chunks.Take());
                    sw.WriteLine(chunk);
                }
                request = sr.ReadLine();
            }

            clientSocket.Close();
            Console.ReadKey();

        
        }
    }
}
