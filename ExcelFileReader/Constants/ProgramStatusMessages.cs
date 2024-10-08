﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFileReader.Constants
{
    internal static class ProgramStatusMessages
    {
        internal const string UploadingAllowed = "You can select file";
        internal const string FileUsedByAnotherProgram = "This file is currently being used by another application";
        internal const string DataParseToExcelSuccess = "Saving excel file";
        internal const string NoDataToSave = "No data to save";
        internal const string WaitingForServerResponse = "Waiting for server response";
        internal const string WaitingForDataGridUpdating = "Waiting for the data grid to update";
        internal const string DataSaveSuccess = "Data saved successfully";
        internal const string DataUpdateSuccess = "Data updeted successfully";
        internal const string DataDeleteSuccess = "Data deleted successfully";
        internal const string SelectValidData = "Please, select valid data";
        internal const string UnexpectedError = "Unexpected error";
    }
}
