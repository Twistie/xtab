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

        struct recordBlock
        {
            public UInt16[][] classifications;
        }

        static int NUMBER_RECORDS = 1000000000;
        static int NUMBER_RECORD_BLOCKS = NUMBER_RECORDS / 16;
        static int[] CLASSIFICATIONS = { 4, 5, 5 };
        static int[] CLASSIFICATION_OFFSETS = { 0, 4, 9};
        static recordBlock[] records;
        static UInt16[] bitMasks;
        static int[] numberOnesLookup;
        
        static void Main(string[] args)
        {
            populateRecords();
            Console.WriteLine("Starting init");
            init();
            Console.WriteLine("Finished init");
            
            Console.WriteLine("First record :" + records[0].classifications.ToString());
            while (true)
            {
                int c = xtab();
                Console.WriteLine("RecordCount: " + c);
                Console.ReadKey();
            }
        }

        static void init()
        {
            bitMasks = new UInt16[16];
            for (int i = 0; i < 16; i ++  )
            {
                bitMasks[i] = (UInt16)Math.Pow(2,i);
            }
            numberOnesLookup = new int[UInt16.MaxValue];
            for( UInt16 i = 0; i < UInt16.MaxValue; i++ )
            {
                int count = 0;
                for(  int n = 0; n < 16; n ++ )
                    if( GetBit(i , n ) )
                        count ++;
                numberOnesLookup[i] = count;
            }

        }

        private static bool GetBit(UInt16 bits, int offset) {
            return (bits & (1<<offset)) != 0;
        }
        /**
         *  returns an array of records NUMBER_RECORDS long, with a number of single selection classifications arranged as CLASSIFICATIONS 
         * 
         */
        static void populateRecords() 
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            records = new recordBlock[NUMBER_RECORD_BLOCKS];

            for (int i = 0; i < NUMBER_RECORD_BLOCKS; i++)
            {
                records[i] = new recordBlock();
                records[i].classifications = new UInt16[3][];
                records[i].classifications[0] = new UInt16[4];
                records[i].classifications[1] = new UInt16[5];
                records[i].classifications[2] = new UInt16[5];

                for( int n = 0; n < 16; n ++ )
                {
                    records[i].classifications[0][rand.Next(4)] += (UInt16)Math.Pow(2, n);
                    records[i].classifications[1][rand.Next(5)] += (UInt16)Math.Pow(2, n);
                    records[i].classifications[2][rand.Next(5)] += (UInt16)Math.Pow(2, n);
                }
            }
        }

        static int xtab() 
        {
            int count = 0;
            //The following is a definition of the cell we want, that is, the first option for each classification is true
            int[] classMap = new  int[] {0, 0, 0};
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            foreach( recordBlock rec in records)
            {
               count += numberOnesLookup[rec.classifications[0][0] &
                                         rec.classifications[1][0] &
                                         rec.classifications[2][0]];
            }

            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);
            return count;
        }
    }
}
