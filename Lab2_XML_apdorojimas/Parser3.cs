using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;

namespace Parseris3
{
    class Program
    {
        static void Main(string[] args)
        {
            DataSet ds = new DataSet(); //sukuriamas Dataset objektas

            ds.ReadXml("Studentai.xml"); //nuskaitomas XML failas ir suformuojamos lentelės

            DataSetInfo.DisplayInfo(ds); //atvaizduojam nuskaitytus duomenis

            Studentai s = new Studentai(); //sukuriamas objektas studentų saugojimui

            foreach (DataRow row in ds.Tables["studentas"].Rows)  //imam atitinkamos lentelės eilutes
            {
                Studentas tc = new Studentas();           //sukuriamas objektas studentų duomenų saugojimui

                tc.id = row["id"].ToString();             //nuskaitom "id" reikšmę
                tc.vardas = row["vardas"].ToString();     //nuskaitom "vardas" reikšmę
                tc.vidurkis = row["vidurkis"].ToString(); //nuskaitom "vidurkis" reikšmę

                DataRow[] children = row.GetChildRows("studentas_pazymiai"); // pagal lentelių ryšį, imama dukterinės lentelės eilutė

                tc.paz1 = (children[0]["paz1"]).ToString(); //nuskaitom "paz1" reikšmę
                tc.paz2 = (children[0]["paz2"]).ToString(); //nuskaitom "paz2" reikšmę

                s.items.Add(tc);  // įtraukiam studento objektą į studentai 
            }

            s.Display(); //atvaizduojami duomenys


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

    public class DataSetInfo
    {
        public static void DisplayInfo(DataSet ds) //metodas DataSet objekto duomenų atvaizdavimui
        {
            foreach (DataTable dt in ds.Tables)
            {
                Console.WriteLine("\n===============================================");
                Console.WriteLine("Table = " + dt.TableName + "\n");
                foreach (DataColumn dc in dt.Columns)
                {
                    Console.Write("{0,-14}", dc.ColumnName);
                }
                Console.WriteLine("\n-----------------------------------------------");

                foreach (DataRow dr in dt.Rows)
                {
                    foreach (object data in dr.ItemArray)
                    {
                        Console.Write("{0,-14}", data.ToString());

                    }
                    Console.WriteLine();
                }
                Console.WriteLine("===============================================");
            } 

            foreach (DataRelation dr in ds.Relations)
            {
                Console.WriteLine("\n\nRelations:");
                Console.WriteLine(dr.RelationName + "\n\n");
            }

        } 
    }
}
