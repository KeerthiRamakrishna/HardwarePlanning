using Npgsql;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;

class Program
{
    static void Main(string[] args)
    {
        var excelFilePath = "C:\\2023\\DotNetExperiments\\OSPorting\\Import\\ImportDataAutoCore\\ImportExcel\\ImportExcel\\Os_Dates_Treville.xlsx"; // Replace with your Excel file path

        DataTable dtTable = new DataTable();
        using (var stream = new FileStream(excelFilePath, FileMode.Open))
        {
            stream.Position = 0;
            IWorkbook workbook = new XSSFWorkbook(stream);
            ISheet sheet = workbook.GetSheet("HW_Planning");//GetSheetAt(2); // Assuming data is in the first sheet

            // Read header row (assumes it's the first row)
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            // Create columns in DataTable based on header row
            for (int cell = 0; cell < cellCount; cell++)
            {
                dtTable.Columns.Add(headerRow.GetCell(cell)?.ToString() ?? $"Column{cell + 1}");
            }

            // Read data rows
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                IRow currentRow = sheet.GetRow(row);
                DataRow dataRow = dtTable.NewRow();

                for (int cell = 0; cell < cellCount; cell++)
                {
                    dataRow[cell] = currentRow.GetCell(cell)?.ToString() ?? string.Empty;
                }

                dtTable.Rows.Add(dataRow);
            }
        }
        var connectionString = "Host=denue6pr331;User ID=postgres;Password=pass;Timeout=30;Database=OSPortDB"; // Replace with your connection string

        // Process the DataTable (e.g., display or manipulate the data)
        foreach (DataRow row in dtTable.Rows)
        {

            using (var connection = new NpgsqlConnection(connectionString))
            {
                int SInum = 0;
                string ProgramIncrement = "";
                int StatusID = 0;
                int ArchitectureID = 0;
                int DerivativeID = 0;
                int TestPCID = 0;
                string HWAssetNo = "";
                string HWevaluationBoard = "";
                string MCU = "";
                int PersonID = 0;
                string  StartWeek = "";
                string  EndWeek = "";
                DateTime? StartDate = null;
                DateTime? EndDate = null;
                string Comments = "";

                //SInum = row.ItemArray[0].ToString();
                ProgramIncrement = row.ItemArray[0].ToString();
                connection.Open();
                using var cmdstatusID = new NpgsqlCommand("SELECT  \"StatusID\" \tFROM public.\"Status\" where \t\"StatusName\" = '" + row.ItemArray[1].ToString() + "'", connection);
                using var reader = cmdstatusID.ExecuteReader();
                if (reader.Read())
                {
                    // Access data from columns (by column name or index)
                    StatusID = reader.GetInt32(0); // Assuming Column1 is an integer
                    reader.Close(); // Close the first data reader
                }
                else
                {
                    reader.Close(); // Close the first data reader
                    using var cmd1 = new NpgsqlCommand("INSERT INTO public.\"Status\"(\"StatusName\") VALUES ('" + row.ItemArray[1].ToString() + "')", connection);
                    cmd1.ExecuteNonQuery();
                    using var cmdstatus = new NpgsqlCommand("SELECT  \"StatusID\" \tFROM public.\"Status\" where \t\"StatusName\" = '" + row.ItemArray[1].ToString() + "'", connection);
                    using var reader11 = cmdstatus.ExecuteReader();
                    if (reader11.Read())
                    {
                        // Access data from columns (by column name or index)
                        StatusID = reader11.GetInt32(0); // Assuming Column1 is an integer
                    }
                    reader11.Close(); // Close the first data reader
                }

                using var cmdArchitectureID = new NpgsqlCommand("select \"ArchitectureID\" from public.\"Architecture\"\r\n where \"ArchitectureName\" = '" + row.ItemArray[2].ToString() + "'", connection);
                using var reader2 = cmdArchitectureID.ExecuteReader();
               
                if (reader2.Read())
                {
                    // Access data from columns (by column name or index)
                    ArchitectureID = reader2.GetInt32(0);
                    reader2.Close(); // Close the first data reader
                }
                else
                {
                    reader2.Close(); // Close the first data reader
                    using var cmd1 = new NpgsqlCommand("INSERT INTO public.\"Architecture\" (\"ArchitectureName\") VALUES ('" + row.ItemArray[2].ToString() + "')", connection);
                    cmd1.ExecuteNonQuery();
                    using var cmdArchitect2 = new NpgsqlCommand("select \"ArchitectureID\" from public.\"Architecture\" where \"ArchitectureName\" = '" + row.ItemArray[2].ToString() + "'", connection);
                    using var reader22 = cmdArchitect2.ExecuteReader();
                    if (reader22.Read())
                    {
                        // Access data from columns (by column name or index)
                        ArchitectureID = reader22.GetInt32(0); // Assuming Column1 is an integer
                    }
                    reader22.Close(); // Close the first data reader
                }
                
                using var cmdDerivative = new NpgsqlCommand("SELECT \"DerivativeID\" FROM public.\"Derivative\" where \"DerivativeName\" = '" + row.ItemArray[3].ToString() + "'", connection);
                using var reader3 = cmdDerivative.ExecuteReader();
                if (reader3.Read())
                {
                    // Access data from columns (by column name or index)
                    DerivativeID = reader3.GetInt32(0);
                    reader3.Close(); // Close the first data reader
                }
                else
                {
                    reader3.Close(); // Close the first data reader
                    using var cmd1 = new NpgsqlCommand("INSERT INTO public.\"Derivative\"(\"DerivativeName\") VALUES ('" + row.ItemArray[3].ToString() + "')", connection);
                    cmd1.ExecuteNonQuery();
                    using var cmdAvailabilityStatus3 = new NpgsqlCommand("SELECT \"DerivativeID\" FROM public.\"Derivative\" where \"DerivativeName\" = '" + row.ItemArray[3].ToString() + "'", connection);
                    using var reader33 = cmdAvailabilityStatus3.ExecuteReader();
                    if (reader33.Read())
                    {
                        // Access data from columns (by column name or index)
                        DerivativeID = reader33.GetInt32(0); // Assuming Column1 is an integer
                    }
                    reader33.Close(); // Close the first data reader
                }
                
                using var cmdTestPCID = new NpgsqlCommand("select \"TestPCID\" from public.\"TestPC\" where \"TestPCName\" = '" + row.ItemArray[4].ToString() + "'", connection);
                using var reader4= cmdTestPCID.ExecuteReader();
                if (reader4.Read())
                {
                    // Access data from columns (by column name or index)
                    TestPCID = reader4.GetInt32(0);
                    reader4.Close(); // Close the first data reader
                }
                else
                {
                    reader4.Close(); // Close the first data reader
                    using var cmd1 = new NpgsqlCommand("INSERT INTO public.\"TestPC\"(\"TestPCName\") VALUES ('" + row.ItemArray[4].ToString() + "')", connection);
                    cmd1.ExecuteNonQuery();
                    using var cmdcomVendor3 = new NpgsqlCommand("select \"TestPCID\" from public.\"TestPC\" where \"TestPCName\" = '" + row.ItemArray[4].ToString() + "'", connection);
                    using var reader33 = cmdcomVendor3.ExecuteReader();
                    if (reader33.Read())
                    {
                        // Access data from columns (by column name or index)
                        TestPCID = reader33.GetInt32(0); // Assuming Column1 is an integer
                    }
                    reader33.Close(); // Close the first data reader
                }

                HWAssetNo = row.ItemArray[5].ToString();
                HWevaluationBoard = row.ItemArray[6].ToString();
                MCU = row.ItemArray[7].ToString();


                using var cmdCompilerVersionID = new NpgsqlCommand("select \"PersonID\" from public.\"Person\" where \"PersonFirstName\" =  '" + row.ItemArray[8].ToString() + "'", connection);
                using var reader5 = cmdCompilerVersionID.ExecuteReader();
                
                if (reader5.Read())
                {
                    // Access data from columns (by column name or index)
                    PersonID = reader5.GetInt32(0);
                    reader5.Close(); // Close the first data reader
                }
                else
                {
                    reader5.Close(); // Close the first data reader
                    using var cmd1 = new NpgsqlCommand("INSERT INTO public.\"Person\"(\"PersonFirstName\")\tVALUES ('" + row.ItemArray[8].ToString() + "')", connection);
                    cmd1.ExecuteNonQuery();
                    using var cmdVersion3 = new NpgsqlCommand("select \"PersonID\" from public.\"Person\" where \"PersonFirstName\" = '" + row.ItemArray[8].ToString() + "'", connection);
                    using var reader55 = cmdVersion3.ExecuteReader();
                    if (reader55.Read())
                    {
                        // Access data from columns (by column name or index)
                        PersonID = reader55.GetInt32(0); // Assuming Column1 is an integer
                    }
                    reader55.Close(); // Close the first data reader
                }

                StartWeek = row.ItemArray[9].ToString();
                EndWeek = row.ItemArray[10].ToString();

                string startdateString = row.ItemArray[11].ToString();
                StartDate = !string.IsNullOrEmpty(startdateString) ? Convert.ToDateTime(startdateString) : (DateTime?)null;
                //StartDate = Convert.ToDateTime(row.ItemArray[11].ToString()); // Replace with your actual data

                string enddateString = row.ItemArray[12].ToString();
                EndDate = !string.IsNullOrEmpty(enddateString) ? Convert.ToDateTime(enddateString) : (DateTime?)null;

                //EndDate = Convert.ToDateTime(row.ItemArray[12].ToString()); // Replace with your actual data
                Comments = row.ItemArray[13].ToString(); // Replace with your actual data

                using var cmd = new NpgsqlCommand("INSERT INTO public.\"HardwarePlanning\"(\"ProgramIncrement\", \"StatusID\", \"ArchitectureID\", \"DerivativeID\", \"TestPCID\", \"HWAssetNo\", \"HWevaluationBoard\", \"MCU\", \"PersonID\", \"StartWeek\", \"EndWeek\", \"StartDate\", \"EndDate\",\"Comments\") " +
                    "VALUES (@ProgramIncrement, @StatusID,@ArchitectureID,@DerivativeID,@TestPCID,@HWAssetNo,@HWevaluationBoard,@MCU,@PersonID,@StartWeek,@EndWeek,@StartDate,@EndDate,@Comments)", connection);
                cmd.Parameters.AddWithValue("ProgramIncrement", ProgramIncrement);
                cmd.Parameters.AddWithValue("StatusID", StatusID);
                cmd.Parameters.AddWithValue("ArchitectureID", ArchitectureID);
                cmd.Parameters.AddWithValue("DerivativeID", DerivativeID);
                cmd.Parameters.AddWithValue("TestPCID", TestPCID);
                cmd.Parameters.AddWithValue("HWAssetNo", HWAssetNo);
                cmd.Parameters.AddWithValue("HWevaluationBoard", HWevaluationBoard);
                cmd.Parameters.AddWithValue("MCU", MCU);
                cmd.Parameters.AddWithValue("PersonID", PersonID);
                cmd.Parameters.AddWithValue("StartWeek", StartWeek);
                cmd.Parameters.AddWithValue("EndWeek", EndWeek);
                cmd.Parameters.AddWithValue("StartDate", StartDate);
                cmd.Parameters.AddWithValue("EndDate", EndDate);
                cmd.Parameters.AddWithValue("Comments", Comments);



                cmd.ExecuteNonQuery();

            }







        }
    }
}
