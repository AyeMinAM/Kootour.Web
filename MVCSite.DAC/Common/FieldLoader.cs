using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using WMath.Facilities;
using MVCSite.Common;

namespace MVCSite.DAC.Common
{

    public static class FieldLoaderHelper
    {
        private static Dictionary<string, Dictionary<string, int>> indexCache = new Dictionary<string, Dictionary<string, int>>(350 << 3, StringComparer.InvariantCultureIgnoreCase);//Æ½¾ùÒ»¸ö¹ý³Ì¶ÔÓ¦Ê¹ÓÃ8¸öÀà

        public static Dictionary<string, int> Get(string key)
        {
            if (indexCache.ContainsKey(key))
            {
                return indexCache[key];
            }
            return null;
        }

        public static void Add(string key, Dictionary<string, int> nameIndexs)
        {
            if (indexCache.ContainsKey(key))
            {
                indexCache[key] = nameIndexs;
            }
            else
            {
                indexCache.Add(key, nameIndexs);
            }
        }

    }
    /// <summary>
    /// A helper class to load value from datarecord(datareader).
    /// </summary>
    public sealed class FieldLoader : IDisposable
    {
        //private string [] fields;
        private Dictionary<string, int> nameIndexs;
        private IDataRecord record;
        private string spName = string.Empty;

        /// <summary>
        /// Get values from datarecord.
        /// </summary>
        /// <param name="record">The input datarecord.</param>
        public FieldLoader(IDataRecord record)
        {
            this.record = record;
            int count = record.FieldCount;
            //fields = new string [count];
            //FieldLoaderHelper.Get ( ( ( System.Data.SqlClient.SqlDataReader ) ( record ) ).Command.CommandText );
            nameIndexs = new Dictionary<string, int>(64, StringComparer.InvariantCultureIgnoreCase);
            for (int i = 0; i < count; i++)
            {
                string name = record.GetName(i);
                //fields [i] = name;
                nameIndexs.Add(name, i);
            }
        }

        //public FieldLoader( IDataRecord record, string spName, string className )
        //{
        //    this.record = record;
        //    int count = record.FieldCount;
        //    string key = spName + "|" + className;
        //    nameIndexs = FieldLoaderHelper.Get ( key );
        //    if ( nameIndexs == null )
        //    {
        //        nameIndexs = new Dictionary<string, int> ( 64 );
        //        for ( int i = 0; i < count; i++ )
        //        {
        //            string name = record.GetName ( i );
        //            nameIndexs.Add ( name.ToLower (), i );
        //        }
        //        FieldLoaderHelper.Add( key, nameIndexs );
        //    }
        //}

        /// <summary>
        /// Free Reference to recordset
        /// 
        /// </summary>
        /// <param name="record">NONE</param>
        public void FreeRecordSet()
        {
            this.record = null;
            this.nameIndexs = null;

        }

        /// <summary>
        /// Get ordinal in the datarecord of specific field name.
        /// </summary>
        /// <param name="name">Name of the column.</param>
        /// <returns>Ordinal of specific field name.</returns>
        private int GetOrdinal(string key)
        {
            //int count = fields.Length;
            //for ( int i = 0; i < count; i++ )
            //{
            //    if ( fields [i] == name || string.Compare ( fields [i], name, true ) == 0 )
            //    {
            //        return i;
            //    }
            //}
            //string key = name.ToLower ();
            if (nameIndexs.ContainsKey(key))
            {
                return nameIndexs[key];
            }
            return -1;
            //object o = nameIndexs[ name ];
            //if ( o != null )
            //    return Convert.ToInt32( o );
            //return -1;
        }

        #region GetField Method

        /// <summary>
        /// Gets a boolean value from the datarecord.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="false" /> for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public bool GetBoolean(string name)
        {
            int i = GetOrdinal(name);

            if (i < 0 || record.IsDBNull(i))
                return false;
            else
                return record.GetBoolean(i);
        }

        /// <summary>
        /// Gets a byte value from the datarecord.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public byte GetByte(string name)
        {
            int i = GetOrdinal(name);

            if (i < 0 || record.IsDBNull(i))
                return 0;
            else
                return record.GetByte(i);
        }

        /// <summary>
        /// Invokes the GetBytes method of the underlying datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public long GetBytes(string name, long fieldOffset, byte[] buffer, int bufferOffset, int length)
        {
            int i = GetOrdinal(name);

            if (i < 0 || record.IsDBNull(i))
                return 0;
            else
                return record.GetBytes(i, fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Gets a char value from the datarecord.
        /// </summary>
        /// <remarks>
        /// Returns Char.MinValue for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public char GetChar(string name)
        {
            int i = GetOrdinal(name);

            if (i < 0 || record.IsDBNull(i))
                return char.MinValue;
            else
                return record.GetChar(i);
        }

        /// <summary>
        /// Invokes the GetChars method of the underlying datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public long GetChars(string name, long fieldOffset, char[] buffer, int bufferOffset, int length)
        {
            int i = GetOrdinal(name);

            if (i < 0 || record.IsDBNull(i))
                return 0;
            else
                return record.GetChars(i, fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Gets a date value from the datarecord.
        /// </summary>
        /// <remarks>
        /// Returns DateTime.MinValue for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public DateTime GetDateTime(string name)
        {
            int i = GetOrdinal(name);

            if (i < 0 || record.IsDBNull(i))
                return DateTime.MinValue;
            else
                return record.GetDateTime(i);
        }

        /// <summary>
        /// Gets a decimal value from the datarecord.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public decimal GetDecimal(string name)
        {
            int i = GetOrdinal(name);

            if (i < 0 || record.IsDBNull(i))
                return 0;
            else
                return record.GetDecimal(i);
        }

        /// <summary>
        /// Gets a double from the datarecord.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public double GetDouble(string name)
        {
            int i = GetOrdinal(name);

            if (i < 0 || record.IsDBNull(i))
                return 0;
            else
                return record.GetDouble(i);
        }

        /// <summary>
        /// Gets a Single value from the datarecord.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public float GetFloat(string name)
        {
            int i = GetOrdinal(name);

            if (i < 0 || record.IsDBNull(i))
                return 0;
            else
                return record.GetFloat(i);
        }

        /// <summary>
        /// Gets a Guid value from the datarecord.
        /// </summary>
        /// <remarks>
        /// Returns Guid.Empty for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public Guid GetGuid(string name)
        {
            int i = GetOrdinal(name);

            if (i < 0 || record.IsDBNull(i))
                return Guid.Empty;
            else
                return record.GetGuid(i);
        }

        /// <summary>
        /// Gets a short value from the datarecord.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public short GetInt16(string name)
        {
            int i = GetOrdinal(name);

            if (i < 0 || record.IsDBNull(i))
                return 0;
            else
                return record.GetInt16(i);
        }

        /// <summary>
        /// Gets an integer from the datarecord.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public int GetInt32(string name)
        {
            int i = GetOrdinal(name);

            if (i < 0 || record.IsDBNull(i))
                return 0;
            else
                return record.GetInt32(i);
        }

        /// <summary>
        /// Gets a long value from the datarecord.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public long GetInt64(string name)
        {
            int i = GetOrdinal(name);

            if (i < 0 || record.IsDBNull(i))
                return 0;
            else
                return record.GetInt64(i);
        }

        /// <summary>
        /// Gets a string value from the datarecord.
        /// </summary>
        /// <remarks>
        /// Returns empty string for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public string GetString(string name)
        {
            int i = GetOrdinal(name);

            if (i < 0 || record.IsDBNull(i))
                return string.Empty;
            else
                return record.GetString(i);
        }

        #endregion

        #region LoadField Methods

        /// <summary>
        /// Loads a boolean value from the datarecord.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="field">Field to load.</param>
        public bool LoadBoolean(string name)
        {
            int i = GetOrdinal(name);
            bool field;
            if (i > -1 && !record.IsDBNull(i))
                field = record.GetBoolean(i);
            else
                field = false;
            return field;
        }

        /// <summary>
        /// Loads a byte value from the datarecord.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="field">Field to load.</param>
        public byte LoadByte(string name)
        {
            int i = GetOrdinal(name);
            byte field = 0;
            if (i > -1 && !record.IsDBNull(i))
                field = record.GetByte(i);
            return field;
        }

        /// <summary>
        /// Invokes the GetBytes method of the underlying datarecord.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="field">Field to load.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public long LoadBytes(string name, long fieldOffset, byte[] buffer, int bufferOffset, int length)
        {
            int i = GetOrdinal(name);
            long field = 0;
            if (i > -1 && !record.IsDBNull(i))
                field = record.GetBytes(i, fieldOffset, buffer, bufferOffset, length);
            return field;
        }

        /// <summary>
        /// Loads a char value from the datarecord.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="field">Field to load.</param>
        public char LoadChar(string name)
        {
            int i = GetOrdinal(name);
            char field = ' ';
            if (i > -1 && !record.IsDBNull(i))
                field = record.GetChar(i);
            return field;
        }

        /// <summary>
        /// Invokes the GetChars method of the underlying datareader.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="field">Field to load.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public long LoadChars(string name, long fieldOffset, char[] buffer, int bufferOffset, int length)
        {
            int i = GetOrdinal(name);
            long field = 0;
            if (i > -1 && !record.IsDBNull(i))
                field = record.GetChars(i, fieldOffset, buffer, bufferOffset, length);
            return field;
        }

        /// <summary>
        /// Loads a date value from the datarecord.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="field">Field to load.</param>
        public DateTime LoadDateTime(string name)
        {
            int i = GetOrdinal(name);
            DateTime field;
            if (i > -1 && !record.IsDBNull(i))
                field = record.GetDateTime(i);
            else
                field = DateTime.MinValue;
            return field;
        }

        /// <summary>
        /// Loads a decimal value from the datarecord.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="field">Field to load.</param>
        public decimal LoadDecimal(string name)
        {
            int i = GetOrdinal(name);
            decimal field = 0;
            if (i > -1 && !record.IsDBNull(i))
                field = record.GetDecimal(i);
            else
                field = 0;
            return field;
        }

        /// <summary>
        /// Loads a double value from the datarecord.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="field">Field to load.</param>
        public double LoadDouble(string name)
        {
            int i = GetOrdinal(name);
            double field = 0;
            if (i > -1 && !record.IsDBNull(i))
                field = record.GetDouble(i);
            else
                field = 0;
            return field;
        }

        /// <summary>
        /// Loads a float value from the datarecord.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="field">Field to load.</param>
        public float LoadFloat(string name)
        {
            int i = GetOrdinal(name);

            float field = 0;
            //if (i > -1 && !record.IsDBNull(i)) field = record.GetFloat(i);
            if (i > -1 && !record.IsDBNull(i))
                field = SafeConvert.ToSingle(record.GetValue(i));
            else
                field = 0;
            return field;

        }

        /// <summary>
        /// Loads a Guid value from the datarecord.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="field">Field to load.</param>
        public Guid LoadGuid(string name)
        {
            int i = GetOrdinal(name);

            Guid field = Guid.Empty;
            if (i > -1 && !record.IsDBNull(i))
            {
                field = record.GetGuid(i);
            }
            return field;
        }

        /// <summary>
        /// Loads a short value from the datarecord.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="field">Field to load.</param>
        public short LoadInt16(string name)
        {
            int i = GetOrdinal(name);
            short field = 0;
            if (i > -1 && !record.IsDBNull(i))
                field = record.GetInt16(i);
            else
                field = 0;
            return field;
        }

        /// <summary>
        /// Loads a integer value from the datarecord.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="field">Field to load.</param>
        public int LoadInt32(string name)
        {
            int i = GetOrdinal(name);
            int field = 0;
            if (i > -1 && !record.IsDBNull(i))
                field = record.GetInt32(i);
            else
                field = 0;
            return field;
        }

        /// <summary>
        /// added by jinxiang. for person's sex.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="field"></param>
        /// <param name="defaultValue"></param>
        public int LoadInt32(string name, int defaultValue)
        {
            int i = GetOrdinal(name);
            int field = 0;
            if (i > -1 && !record.IsDBNull(i))
                field = Convert.ToInt32(record.GetValue(i));
            else
                field = defaultValue;
            return field;
        }

        /// <summary>
        /// Loads a long value from the datarecord.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="field">Field to load.</param>
        public long LoadInt64(string name)
        {
            int i = GetOrdinal(name);
            long field = 0;
            if (i > -1 && !record.IsDBNull(i))
                field = record.GetInt64(i);
            else
                field = 0;
            return field;
        }

        /// <summary>
        /// Loads a string value from the datarecord.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="field">Field to load.</param>
        public string LoadString(string name)
        {
            int i = GetOrdinal(name);
            string field = "";
            if (i > -1 && !record.IsDBNull(i))
                field = record.GetString(i);
            else
                field = string.Empty;
            return field;
        }

        #endregion

        #region Dispose Methods

        private bool disposed = false;

        //public void Dispose()
        //{
        //    Dispose ( true );
        //    GC.SuppressFinalize ( this );
        //}

        //public void Dispose( bool disposing )
        //{
        //    if ( disposed )
        //    {
        //        return;
        //    }
        //    if ( disposing )
        //    {
        //        if ( nameIndexs != null )
        //        {
        //            nameIndexs = null;
        //        }
        //    }
        //    disposed = true;
        //}

        //~FieldLoader()
        //{
        //    //TODO:True or false
        //    //if ( record != null )
        //    record = null;

        //    //this.Dispose ( true );
        //}
        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {

                // Call the appropriate methods to clean up 
                // unmanaged resources here.
                // If disposing is false, 
                // only the following code is executed.
                this.record = null;
            }
            disposed = true;
        }

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method 
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        //~FieldLoader()      
        //{
        //    // Do not re-create Dispose clean-up code here.
        //    // Calling Dispose(false) is optimal in terms of
        //    // readability and maintainability.
        //    Dispose(false);
        //}

        #endregion
    }
}