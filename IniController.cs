using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IniController
{
    /*
        This class can be used in a static manner in order to access it in an easy way anywhere in the application.
        There are two ways to do so. 
            1) use the filepath at each method call.
            2) set the filepath once and read/write to it whenever.
        Or 
            3) the class can be instantiated if several ini files are used simultaneously.

        == Best usage ==

        1) Specify ini file for each call:
            a) using IniController;
            b) Ini.GetString(filename, someCategory, someKey, defaultString);
            ...

        2) One ini file
            a) using IniController;
            b) Ini.SetFile(fullfilename);
            c) Ini.GetString(someCategory, someKey, defaultString);
                !!! Attention !!! in this case, the various methods start with uppercase letters
            ...

        3) Several ini files
            a) using IniController;
            b) Ini colorsIniFile = new Ini(fullfilename);
            c) colorsIniFile.getString(someCategory, someKey, defaultString);
                !!! Attention !!! in this case, the various methods start with lowercase letters
            ...
    */
    class Ini
    {

        #region Non-static elements

        public string File { get; set; }

        private Ini() { }

        public Ini(string file)
        {
            File = file;
        }

        #endregion Non-static elements

        #region Static elements

        private static string staticFile;
        public static void SetFile(string file)
        {
            staticFile = file;
        }

        private const Unique UniqueDefault = Unique.NO;
        private const Order OrderDefault = Order.NONE;

        #endregion Static elements

        #region Read values

        [DllImport("KERNEL32.DLL",
            EntryPoint = "GetPrivateProfileStringW",
            SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            string lpReturnString,
            int nSize,
            string lpFilename);


        private static List<string> GetCategories(string iniFile)
        {
            string returnString = new string(' ', 65536);
            GetPrivateProfileString(null, null, null, returnString, 65536, iniFile);
            List<string> result = new List<string>(returnString.Split('\0'));
            result.RemoveRange(result.Count - 2, 2);
            return result;
        }

        private static List<string> GetKeys(string iniFile, string category)
        {
            string returnString = new string(' ', 32768);
            GetPrivateProfileString(category, null, null, returnString, 32768, iniFile);
            List<string> result = new List<string>(returnString.Split('\0'));
            result.RemoveRange(result.Count-2,2);
            return result;
        }

        #region Read string values
        /********************
                String Values   
                ********************/

        /*** string ***/

        public static string GetString(string iniFile, string category, string key, string defaultValue)
        {
            string returnString = new string(' ', 1024);
            GetPrivateProfileString(category, key, defaultValue, returnString, 1024, iniFile);
            return returnString.Split('\0')[0];
        }

        public static string GetString(string category, string key, string defaultValue)
        {
            return GetString(staticFile, category, key, defaultValue);
        }

        // non-static
        public string getString(string category, string key, string defaultValue)
        {
            return GetString(File, category, key, defaultValue);
        }

        /*** string[]     - default string ***/

        public static string[] GetStringArray(string iniFile, string category, string key, string defaultValue)
        {
            // Extract from ini file
            string returnString = new string(' ', 1024);
            GetPrivateProfileString(category, key, defaultValue, returnString, 1024, iniFile);

            // Parse into array
            string[] returnArray = removeBrackets(returnString.Split('\0')[0]).Split(',');
            for (int i = 0; i < returnArray.Length; i++)
            {
                returnArray[i] = returnArray[i].Trim();
            }
            return returnArray;
        }

        public static string[] GetStringArray(string category, string key, string defaultValue)
        {
            return GetStringArray(staticFile, category, key, defaultValue);
        }

        // non-static
        public string[] getStringArray(string category, string key, string defaultValue)
        {
            return GetStringArray(File, category, key, defaultValue);
        }

        /*** string[]     - default string[] ***/

        public static string[] GetStringArray(string iniFile, string category, string key, string[] defaultArray)
        {
            return GetStringArray(iniFile, category, key, string.Join(",", defaultArray));
        }

        public static string[] GetStringArray(string category, string key, string[] defaultArray)
        {
            return GetStringArray(staticFile, category, key, defaultArray);
        }

        // non-static
        public string[] getStringArray(string category, string key, string[] defaultArray)
        {
            return GetStringArray(File, category, key, defaultArray);
        }

        #endregion Read string values

        #region Read int values

        /********************
                Integer Values   
                ********************/

        /*** int     - default string ***/

        public static int GetInt(string iniFile, string category, string key, string defaultValue)
        {
            try
            {
                return int.Parse(GetString(iniFile, category, key, defaultValue));
            }
            catch (FormatException)
            {
                return int.Parse(defaultValue); ;
            }           
        }

        public static int GetInt(string category, string key, string defaultValue)
        {
            return GetInt(staticFile, category, key, defaultValue);
        }

        // non-static
        public int getInt(string category, string key, string defaultValue)
        {
            return GetInt(File, category, key, defaultValue);
        }

        /*** int     - default int ***/

        public static int GetInt(string iniFile, string category, string key, int defaultInt)
        {
            try
            {
                return int.Parse(GetString(iniFile, category, key, defaultInt.ToString()));
            }
            catch (FormatException)
            {
                return defaultInt;
            }
        }

        public static int GetInt(string category, string key, int defaultInt)
        {
            return GetInt(staticFile, category, key, defaultInt);
        }

        // non-static
        public int getInt(string category, string key, int defaultInt)
        {
            return GetInt(File, category, key, defaultInt);
        }

        /*** int[]   - default string ***/

        public static int[] GetIntArray(string iniFile, string category, string key, string defaultValue)
        {
            // Extract from ini file
            string returnString = new string(' ', 1024);
            GetPrivateProfileString(category, key, defaultValue, returnString, 1024, iniFile);

            // Parse into array
            string[] returnArray = removeBrackets(returnString.Split('\0')[0]).Split(',');         
            int[] returnIntArray = new int[returnArray.Length];

            bool success = true;

            // Parse elements (strings) as integers
            for (int i = 0; i < returnArray.Length; i++)
            {
                try
                {
                    returnIntArray[i] = int.Parse(returnArray[i].Trim());
                }
                catch (FormatException)
                {
                    success = false;
                    break;
                }
            }
            if (!success)
            {
                string[] defaultArray = removeBrackets(defaultValue).Split(',');
                returnIntArray = new int[defaultArray.Length];
                for (int i = 0; i < defaultArray.Length; i++)
                {
                    returnIntArray[i] = int.Parse(defaultArray[i].Trim());
                }
            }

            return returnIntArray;

        }

        public static int[] GetIntArray(string category, string key, string defaultValue)
        {
            return GetIntArray(staticFile, category, key, defaultValue);
        }

        // non-static
        public int[] getIntArray(string category, string key, string defaultValue)
        {
            return GetIntArray(File, category, key, defaultValue);
        }

        /*** int[]   - default string[] ***/

        public static int[] GetIntArray(string iniFile, string category, string key, string[] defaultValue)
        {
            return GetIntArray(iniFile, category, key, string.Join(",", defaultValue));
        }

        public static int[] GetIntArray(string category, string key, string[] defaultValue)
        {
            return GetIntArray(staticFile, category, key, defaultValue);
        }

        // non-static
        public int[] getIntArray(string category, string key, string[] defaultValue)
        {
            return GetIntArray(File, category, key, defaultValue);
        }

        /*** int[]   - default int ***/

        public static int[] GetIntArray(string iniFile, string category, string key, int defaultValue)
        {
            try
            {
                return GetIntArray(iniFile, category, key, "a");
            }
            catch (FormatException)
            {
                return new int[] { defaultValue };
            }
        }

        public static int[] GetIntArray(string category, string key, int defaultValue)
        {
            return GetIntArray(staticFile, category, key, defaultValue);
        }

        // non-static
        public int[] getIntArray(string category, string key, int defaultValue)
        {
            return GetIntArray(File, category, key, defaultValue);
        }


        /*** int[]   - default int[] ***/

        public static int[] GetIntArray(string iniFile, string category, string key, int[] defaultValue)
        {
            try
            {
                return GetIntArray(iniFile, category, key, "a");
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public static int[] GetIntArray(string category, string key, int[] defaultValue)
        {
            return GetIntArray(staticFile, category, key, defaultValue);
        }

        // non-static
        public int[] getIntArray(string category, string key, int[] defaultValue)
        {
            return GetIntArray(File, category, key, defaultValue);
        }

        #endregion Read int values

        #endregion Read values

        #region Write values

        [DllImport("KERNEL32.DLL",
            EntryPoint = "WritePrivateProfileStringW",
            SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int WritePrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpString,
            string lpFilename);

        #region Write values

        /********************
                Write string, string[], int, int[]   
                ********************/
        public static void Write(string iniFile, string category, string key, object value)
        {
            if (value.GetType() == typeof(string))
                WritePrivateProfileString(category, key, (string)value, iniFile);
            else if (value.GetType() == typeof(int))
                WritePrivateProfileString(category, key, ((int)value).ToString(), iniFile);
            else if (value.GetType() == typeof(string[]))
                WritePrivateProfileString(category, key, string.Join(",", (string[])value), iniFile);
            else if (value.GetType() == typeof(int[]))
                WritePrivateProfileString(category, key, string.Join(",", (int[])value), iniFile);
            else
                throw new InvalidCastException();
        }

        public static void Write(string category, string key, object value)
        {
            if (staticFile == null) throw new FileNotDefinedException();
            Write(staticFile, category, key, value);
        }

        public void write(string category, string key, object value)
        {
            if (staticFile == null) throw new FileNotDefinedException();
            Write(File, category, key, value);
        }

        #endregion Write values

        #region Add string to array key

        /********************
                Add string to a key that has an array value  (hack: if string of the form "a, b, c", its like adding an array)
                ********************/
        public static void AddToArray(string iniFile, string category, string key, string value, Unique unique = UniqueDefault, Order order = OrderDefault)
        {
            string[] oldValues = GetStringArray(staticFile, category, key, "");
            if (unique != Unique.NO)
            {
                bool exists = false;
                foreach (string element in oldValues)
                {
                    if (unique == Unique.IGNORECASE)
                        exists |= (element.ToLower().Equals(value.ToLower()));
                    else
                        exists |= (element.Equals(value));
                }
                if (exists) return;
            }

            string[] newValues = new string[oldValues.Length + 1];
            if (order == Order.IGNORECASE || order == Order.CASESENSITIVE)
            {
                newValues[0] = value;
                oldValues.CopyTo(newValues, 1);
                newValues = sortArray(newValues, order);                
            }
            else
            {
                oldValues.CopyTo(newValues, 0);
                newValues[oldValues.Length] = value;
            }

            Write(iniFile, category, key, newValues);
        }

        public static void AddToArray(string category, string key, string value, Unique unique = UniqueDefault, Order order = OrderDefault)
        {
            AddToArray(staticFile, category, key, value, unique, order);
        }

        public void addToArray(string category, string key, string value, Unique unique = UniqueDefault, Order order = OrderDefault)
        {
            AddToArray(File, category, key, value, unique, order);
        }

        // Inverse order of two last arguments (for real, just to make it possible to define Order and not Unique)
        public static void AddToArray(string iniFile, string category, string key, string value, Order order = OrderDefault, Unique unique = UniqueDefault)
        {
            AddToArray(staticFile, category, key, value, unique, order);
        }

        public static void AddToArray(string category, string key, string value, Order order = OrderDefault, Unique unique = UniqueDefault)
        {
            AddToArray(staticFile, category, key, value, unique, order);
        }

        public void addToArray(string category, string key, string value, Order order = OrderDefault, Unique unique = UniqueDefault)
        {
            AddToArray(File, category, key, value, unique, order);
        }

        #endregion Add string to array key

        #region Add int to array key

        /********************
                Add int to a key that has an array value
                ********************/
        // TODO: Does not work with alphabetical order
        public static void AddToArray(string iniFile, string category, string key, int value, bool unique = false, bool sorted = false)
        {
            int[] oldValues = GetIntArray(staticFile, category, key, "");
            if (unique)
            {
                bool isUnique = false;
                foreach (int i in oldValues)
                {
                    isUnique |= (i == value);
                }
                if (!isUnique)
                    return;
            }

            int[] newValues = new int[oldValues.Length+1];
            
            if (!sorted)
            {
                oldValues.CopyTo(newValues, 0);
                newValues[oldValues.Length] = value;
            }
            else
            {
                newValues[0] = value;
                oldValues.CopyTo(newValues, 1);
                newValues = sortArray(newValues);
            }

        }

        public static void AddToArray(string category, string key, int value, bool unique = false, bool sorted = false)
        {
            AddToArray(staticFile, category, key, value, unique, sorted);
        }

        public void addToArray(string category, string key, int value, bool unique = false, bool sorted = false)
        {
            AddToArray(File, category, key, value, unique, sorted);
        }

        #endregion

        #region Remove all occurences from array

        /********************
                Remove string from string array   
                ********************/
        public static void RemoveAllFromArray(string iniFile, string category, string key, string value, CaseSensitive isCS = CaseSensitive.YES)
        {
            string[] values = GetStringArray(iniFile, category, key, "");
            string[] newValue = new string[values.Length];
            int j = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (isCS == CaseSensitive.NO && !values[i].ToUpper().Equals(value.ToUpper()) && !values[i].Equals(""))
                {
                    newValue[j] += values[i];
                    j++;
                }
                else if (isCS == CaseSensitive.YES && !values[i].Equals(value) && !values[i].Equals(""))
                {
                    newValue[j] += values[i];
                    j++;
                }
            }
            string[] result = new string[j];
            for (int i = 0; i < j; i++)
            {
                result[i] = newValue[i];
            }
            Write(iniFile, category, key, result);
        }

        public static void RemoveAllFromArray(string category, string key, string value, CaseSensitive isCS = CaseSensitive.YES)
        {
            RemoveAllFromArray(staticFile, category, key, value, isCS);
        }

        public void removeAllFromArray(string category, string key, string value, CaseSensitive isCS = CaseSensitive.YES)
        {
            RemoveAllFromArray(File, category, key, value, isCS);
        }

        /********************
                Remove int from int array   
                ********************/
        public static void RemoveAllFromArray(string iniFile, string category, string key, int value)
        {
            int[] values = GetIntArray(iniFile, category, key, "");
            int[] newValue = new int[values.Length];
            int j = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != value)
                {
                    newValue[j] += values[i];
                    j++;
                }
            }
            int[] result = new int[j];
            for (int i = 0; i < j; i++)
            {
                result[i] = newValue[i];
            }
            Write(iniFile, category, key, result);
        }

        public static void RemoveAllFromArray(string category, string key, int value)
        {
            RemoveAllFromArray(staticFile, category, key, value);
        }

        public void removeAllFromArray(string category, string key, int value)
        {
            RemoveAllFromArray(File, category, key, value);
        }

        #endregion Remove all occurences from array

        #region Remove first occurence from array

        /********************
                Remove string from string array   
                ********************/
        public static void RemoveFromArray(string iniFile, string category, string key, string value, CaseSensitive isCS = CaseSensitive.YES)
        {
            string[] values = GetStringArray(iniFile, category, key, "");
            string[] newValue = new string[values.Length];
            int j = 0;
            bool found = false;
            for (int i = 0; i < values.Length; i++)
            {
                if ((isCS == CaseSensitive.NO && !values[i].ToUpper().Equals(value.ToUpper()) && !values[i].Equals("")) || found)
                {
                    newValue[j] += values[i];
                    j++;
                }
                else if (isCS == CaseSensitive.YES && !values[i].Equals(value) && !values[i].Equals(""))
                {
                    newValue[j] += values[i];
                    j++;
                }
                else
                {
                    found = true;
                }
            }
            string[] result = new string[j];
            for (int i = 0; i < j; i++)
            {
                result[i] = newValue[i];
            }
            Write(iniFile, category, key, result);
        }

        public static void RemoveFromArray(string category, string key, string value, CaseSensitive isCS = CaseSensitive.YES)
        {
            RemoveAllFromArray(staticFile, category, key, value, isCS);
        }

        public void removeFromArray(string category, string key, string value, CaseSensitive isCS = CaseSensitive.YES)
        {
            RemoveAllFromArray(File, category, key, value, isCS);
        }

        /********************
                Remove int from int array   
                ********************/
        public static void RemoveFromArray(string iniFile, string category, string key, int value)
        {
            int[] values = GetIntArray(iniFile, category, key, "");
            int[] newValue = new int[values.Length];
            int j = 0;
            bool found = false;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != value || found)
                {
                    newValue[j] += values[i];
                    j++;
                }
                else
                {
                    found = true;
                }
            }
            int[] result = new int[j];
            for (int i = 0; i < j; i++)
            {
                result[i] = newValue[i];
            }
            Write(iniFile, category, key, result);
        }

        public static void RemoveFromArray(string category, string key, int value)
        {
            RemoveAllFromArray(staticFile, category, key, value);
        }

        public void removeFromArray(string category, string key, int value)
        {
            RemoveAllFromArray(File, category, key, value);
        }

        #endregion Remove first occurence from array

        #endregion Write values

        #region Remove key or section

        public static void RemoveKey(string iniFile, string category, string key)
        {
            WritePrivateProfileString(category, key, null, iniFile);
        }

        public static void RemoveKey(string category, string key)
        {
            WritePrivateProfileString(category, key, null, staticFile);
        }

        public void removeKey(string iniFile, string category, string key)
        {
            WritePrivateProfileString(category, key, null, File);
        }

        public static void RemoveSection(string iniFile, string category)
        {
            WritePrivateProfileString(category, null, null, iniFile);
        }

        public static void RemoveSection(string category)
        {
            WritePrivateProfileString(category, null, null, staticFile);
        }

        public void removeSection(string category)
        {
            WritePrivateProfileString(category, null, null, File);
        }

        #endregion Remove key or section

        #region Private helpers

        private static string removeBrackets(string original)
        {
            original = original.Trim();
            string cleaned = original;
            if ((original.IndexOf("{") == 0 && original.IndexOf("}") == original.Length - 1)
                || (original.IndexOf("[") == 0 && original.IndexOf("]") == original.Length - 1)
                || (original.IndexOf("(") == 0 && original.IndexOf(")") == original.Length - 1))
                cleaned = original.Substring(1, original.Length - 2);

            return cleaned;
        }

        // Using bubble sort because mainly sorted small arrays
        private static string[] sortArray(string[] array, Order order = Order.CASESENSITIVE)
        {
            bool sorted = false;
            while (!sorted)
            {
                sorted = true;
                for (int i = 0; i < array.Length-1; i++)
                {
                    if (order == Order.CASESENSITIVE && array[i].CompareTo(array[i+1]) > 0)
                    {
                        string temp = array[i + 1];
                        array[i + 1] = array[i];
                        array[i] = temp;
                        sorted = false;
                    }
                    else if (order == Order.IGNORECASE && array[i].ToLower().CompareTo(array[i + 1].ToLower()) > 0)
                    {
                        string temp = array[i + 1];
                        array[i + 1] = array[i];
                        array[i] = temp;
                        sorted = false;
                    }
                }
            }
            return array;
        }

        // Using bubble sort because mainly sorted small arrays
        private static int[] sortArray(int[] array)
        {
            bool sorted = false;
            while (!sorted)
            {
                sorted = true;
                for (int i = 0; i < array.Length - 1; i++)
                {
                    if (array[i] > array[i + 1])
                    {
                        int temp = array[i + 1];
                        array[i + 1] = array[i];
                        array[i] = temp;
                        sorted = false;
                    }
                    
                }
            }
            return array;
        }

        #endregion Private helpers

    }

    #region Enums

    public enum Order
    {
        NONE, IGNORECASE, CASESENSITIVE
    }

    public enum Unique
    {
        NO, IGNORECASE, CASESENSITIVE
    }

    public enum CaseSensitive
    {
        YES, NO
    }

    #endregion Enums

    #region Exceptions

    public class FileNotDefinedException : Exception
    {
        public FileNotDefinedException()
        : base("File was not found.")
        { }

        public FileNotDefinedException(string filename)
        : base("File " + filename + " was not found.")
        { }

        public FileNotDefinedException(string format, params object[] args)
        : base(string.Format(format, args))
        { }
    }

    #endregion Exceptions

}
