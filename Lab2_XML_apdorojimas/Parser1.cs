using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


namespace Parseris
{
    class Program
    {
        static void Main(string[] args)
        {

            Studentai s = new Studentai();  //sukuriamas objektas studentų saugojimui

            XmlTextReader xtr = new XmlTextReader("Studentai.xml"); //sukuriamas XmlTextReader objektas
            xtr.WhitespaceHandling = WhitespaceHandling.None;  
            xtr.Read(); // perskaitom XML failo deklaraciją ir atsiduriam ties <studentai> elemento žyme

            while (!xtr.EOF) 
            {
                if (xtr.Name == "studentai" && !xtr.IsStartElement()) break;

                while (xtr.Name != "studentas" || !xtr.IsStartElement())
                
                xtr.Read(); // perskaitom <studentai> elemento žymę ir atsiduriam ties <studentas> elemento žyme

                Studentas tc = new Studentas();  //sukuriamas objektas studentų duomenų saugojimui

                tc.id = xtr.GetAttribute("id");          //nuskaitom "id" reikšmę
                tc.vardas = xtr.GetAttribute("vardas");  //nuskaitom "vardas" reikšmę
                xtr.Read();                              // atsiduriam ties <pazymiai> elemento žyme
                xtr.Read();                              // atsiduriam ties <paz1> elemento žyme
                tc.paz1 = xtr.ReadElementString("paz1"); //nuskaitom "paz1" reikšmę
                tc.paz2 = xtr.ReadElementString("paz2"); //nuskaitom "paz2" reikšmę
                xtr.Read();                              // atsiduriam ties <vidurkis> elemento žyme
                tc.vidurkis = xtr.ReadElementString("vidurkis"); // nuskaitom "vidurkis" reikšmę
                // dabar ties </studentas> elemento pabaigos žyme
                s.items.Add(tc); // įtraukiam studento objektą į studentai objektą 
                xtr.Read(); // dabar ties <studentas> elemento žyme arba </studentai> elemento pabaigos žyme
            } 

            xtr.Close(); //uzdarom
            s.Display(); //atvaizduojam gautus duomenis


        }
    }

    public class Studentas
    {
        public string id;
        public string vardas;
        public string paz1;
        public string paz2;
        public string vidurkis;
    }

    public class Studentai
    {
        public ArrayList items = new ArrayList();
        public void Display()
        {
            foreach (Studentas tc in items)
            {
                Console.Write(tc.id + " " + tc.vardas + " " + tc.paz1 + " ");
                Console.WriteLine(tc.paz2 + " " + tc.vidurkis);
                
            }

            Console.ReadLine();
        }
    }
}
