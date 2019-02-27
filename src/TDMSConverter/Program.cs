/*
 * Program name:        TDMS Reader
 * 
 * Purpose:             To read a TDMS file and manipulate the DateTime variable
 *                      to output the most accurate value of the raw data.
 * 
 * Programmer:          NI Instruments
 * 
 * Program Additions:   Stephen M.
 * 
 * Last Additions:      Github v.2301:  Added extra DateTime manipulations
 *                                      due to needing the most accurate 
 *                                      DateTime values for all tdms properties.
 * 
 * 
 * Date last edited:    26 Feb 2019
 * 
 * 
 * Notes:               Program edited for the purposes for UVic Eng Department - M. Aigner test data
 * 
 * This program is property of NI Instruments (c) 2019
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMSConverter
{
    class Program
    {


        /* ReadFile Subroutine
         * 
         * Purpose: Reads the Trial 31 file and outputs the data requested
         * 
         * Input: (int) number of lines of data requested
         * 
         * Output: (void) dumps raw data into text file on the C:\
         */
        public void ReadFile(int dataRequested)
        {
            using (var output = new System.IO.StreamWriter(System.IO.File.Create(@"C:\tdms.overvw.txt")))
            using (var tdms = new NationalInstruments.Tdms.File("trial_31.tdms"))
            {
                tdms.Open();



                output.WriteLine("Properties:");
                foreach (var property in tdms.Properties)
                    output.WriteLine("  {0}: {1}", property.Key, property.Value);
                output.WriteLine();
                foreach (var group in tdms)
                {
                    output.WriteLine("    Group: {0}", group.Name);
                    foreach (var property in group.Properties)
                        output.WriteLine("    {0}: {1}", property.Key, property.Value);
                    output.WriteLine();
                    foreach (var channel in group)
                    {
                        output.WriteLine("        Channel: {0}", channel.Name);
                        foreach (var property in channel.Properties)
                        {
                            output.WriteLine("        {0}: {1}", property.Key, property.Value);
                            var temp = property.Key;
                            if (temp.ToString() == "wf_start_time")
                            {
                                DateTime n = Convert.ToDateTime(property.Value);
                                output.WriteLine("        {0}: {1}", "wf_start_time ticks value: ", n.Ticks);
                                output.WriteLine("        {0}: {1}", "(attempt) wf_start_time binary value: ", n.Ticks);
                            }
                            if (temp.ToString() == "NI_ExpStartTimeStamp")
                            {
                                DateTime n = Convert.ToDateTime(property.Value);
                                output.WriteLine("        {0}: {1}", "NI_ExpStartTimeStamp ticks value: ", n.Ticks);
                                output.WriteLine("        {0}: {1}", "(attempt) NI_ExpStartTimeStamp binary value: ", n.ToBinary());
                            }
                            if (temp.ToString() == "NI_ExpTimeStamp")
                            {
                                DateTime n = Convert.ToDateTime(property.Value);
                                output.WriteLine("        {0}: {1}", "NI_ExpTimeStamp ticks value: ", n.Ticks);
                                output.WriteLine("        {0}: {1}", "(attempt) NI_ExpTimeStamp binary value: ", n.ToBinary());
                            }
                        }

                        output.WriteLine();
                    }
                }

                output.WriteLine("Data:");
                foreach (var group in tdms)
                {
                    output.WriteLine("    Group: {0}", group.Name);
                    foreach (var channel in group)
                    {
                        output.WriteLine("    Channel: {0} ({1} data points of type {2})", channel.Name,
                                            channel.DataCount, channel.DataType);
                        foreach (var value in channel.GetData<object>().Take(5))
                        {
                            DateTime result;

                            if (DateTime.TryParse(value.ToString(), out result))
                            {

                                //DateTime* raw = &result;

                                output.Write("          {0}", value);
                                output.WriteLine("          {0} (Binary Value)", Convert.ToString(result.ToBinary(), 2));
                                output.WriteLine("          {0} (Tick Value)", result.Ticks);

                            }
                            else
                            {
                                output.WriteLine("          {0}", value);
                            }

                        }

                        if (channel.DataCount > 20) output.WriteLine("        ...");
                        output.WriteLine();
                    }
                }
            }
        }

        // Main Method
        static void Main(string[] args)
        {
            using (var output = new System.IO.StreamWriter(System.IO.File.Create(@"C:\export.txt")))
            using (var tdms = new NationalInstruments.Tdms.File("trial_31.tdms"))
            {
                // Used this for practise

                //  Testing noise in the NI Tdms example
                //  foreach (var value in tdms.Groups["Noise data"].Channels["Noise_1"].GetData<double>())
                //  output.WriteLine(value);

                tdms.Open();

                try
                {
                    // Attempted to output 5 lines of data
                    Program c = new Program();
                    c.ReadFile(5);
                }
                // Standard exception handling
                catch (Exception e)
                {
                    // output exception to the console and file
                    output.WriteLine("Error has occured: ", e.ToString());
                }

            }


        }


    }

}


