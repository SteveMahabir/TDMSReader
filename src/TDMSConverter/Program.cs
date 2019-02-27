using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMSConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var output = new System.IO.StreamWriter(System.IO.File.Create(@"C:\export.txt")))
            using (var tdms = new NationalInstruments.Tdms.File("trial_31.tdms"))
            {
                tdms.Open();

                // Used this for practise

                //foreach (var value in tdms.Groups["Noise data"].Channels["Noise_1"].GetData<double>())
                  //  output.WriteLine(value);
            }

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
                            }
                            if (temp.ToString() == "NI_ExpStartTimeStamp")
                            {
                                DateTime n = Convert.ToDateTime(property.Value);
                                output.WriteLine("        {0}: {1}", "wf_start_time ticks value: ", n.Ticks);
                            }
                            if (temp.ToString() == "NI_ExpTimeStamp")
                            {
                                DateTime n = Convert.ToDateTime(property.Value);
                                output.WriteLine("        {0}: {1}", "wf_start_time ticks value: ", n.Ticks);
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
    }
}
