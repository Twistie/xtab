using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {

        struct record
        {
            public BitVector32 classifications;
            public record(int[] classes)
            {
                classifications = new BitVector32();
            }
        }

        static int NUMBER_RECORDS = 1000000000;
        static int[] CLASSIFICATIONS = { 4, 5, 5 };
        static int[] CLASSIFICATION_OFFSETS = { 0, 4, 9};
        static record[] records;
        
        static void Main(string[] args)
        {
            populateRecords();
            
            Console.WriteLine("First record :" + records[0].classifications.ToString());
            while (true)
            {
                int c = xtab();
                Console.WriteLine("RecordCount: " + c);
                Console.ReadKey();
            }
        }
        /**
         *  returns an array of records NUMBER_RECORDS long, with a number of single selection classifications arranged as CLASSIFICATIONS 
         * 
         */
        static void populateRecords() 
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            records = new record[NUMBER_RECORDS];
            int[] bitMasks = new int[16];
            for (int i = 0; i < 16; i ++  )
            {
                bitMasks[i] = (int)Math.Pow(2,i);
            }
            for (int i = 0; i < NUMBER_RECORDS; i++)
            {
                records[i] = new record(CLASSIFICATIONS);

                records[i].classifications[bitMasks[CLASSIFICATION_OFFSETS[0] + rand.Next(CLASSIFICATIONS[0])]] = true;
                records[i].classifications[bitMasks[CLASSIFICATION_OFFSETS[1] + rand.Next(CLASSIFICATIONS[1])]] = true;
                records[i].classifications[bitMasks[CLASSIFICATION_OFFSETS[2] + rand.Next(CLASSIFICATIONS[2])]] = true;

            }
        }

        static int xtab() 
        {
            int count = 0;
            //The following is a definition of the cell we want, that is, the first option for each classification is true
            BitVector32 classMap = new BitVector32();
            Stopwatch stopWatch = new Stopwatch();
            classMap[1] = true;
            classMap[(int)Math.Pow(2,4)] = true;
            classMap[(int)Math.Pow(2, 9)] = true;

            stopWatch.Start();
            foreach( record rec in records)
            {
               if (classMap.Data == rec.classifications.Data)
                    count++;
            }

            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);
            return count;

        }
    }
}
