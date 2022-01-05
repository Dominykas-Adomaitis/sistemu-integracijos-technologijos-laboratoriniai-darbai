using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Parseris2
{
    class Program
    {
        static void Main(string[] args)
        {
            Studentai s = new Studentai();   //sukuriamas objektas studentų saugojimui

            XmlDocument xd = new XmlDocument(); //sukuriamas XmlDocument objektas
            xd.Load("Studentai.xml");           //uzkraunamas XML failas

            XmlNodeList nodelist = xd.SelectNodes("/studentai/studentas"); // gaunam visus <studentas> elementus

            foreach (XmlNode node in nodelist)  // kiekvienam <studentas> elementui
            {
                Studentas tc = new Studentas(); //sukuriamas objektas studentų duomenų saugojimui

                tc.id = node.Attributes.GetNamedItem("id").Value;         //nuskaitom "id" reikšmę
                tc.vardas = node.Attributes.GetNamedItem("vardas").Value; //nuskaitom "vardas" reikšmę

                XmlNode n = node.SelectSingleNode("pazymiai"); // gaunam <pazymiai> elementą
                tc.paz1 = n.ChildNodes.Item(0).InnerText;      //nuskaitom "paz1" reikšmę
                tc.paz2 = n.ChildNodes.Item(1).InnerText;      //nuskaitom "paz2" reikšmę

                tc.vidurkis = node.ChildNodes.Item(1).InnerText;  //nuskaitom "vidurkis" reikšmę

                s.items.Add(tc); // įtraukiam studento objektą į studentai 
            }

            s.Display(); //atvaizduojam duomenis


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
