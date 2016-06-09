using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace L2dotNET.Utility
{
    /// <summary>
    /// Files reader helper.
    /// </summary>
    public static class L2FileReader
    {
        /// <summary>
        /// Creates or opens provided file.
        /// </summary>
        /// <param name="FileName">File name.</param>
        /// <param name="DesiredAccess">File access.</param>
        /// <param name="ShareMode">Share mode.</param>
        /// <param name="SecurityAttributes">Security attributes.</param>
        /// <param name="CreationDisposition">Creation disposition.</param>
        /// <param name="FlagsAndAttributes">Flags and attributes.</param>
        /// <param name="hTemplateFile">Template file.</param>
        /// <returns>True, if file was successfully opened, or created, otherwise false.</returns>
        [DllImport("kernel32", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        internal static extern unsafe IntPtr CreateFile(string FileName, uint DesiredAccess, uint ShareMode, uint SecurityAttributes, uint CreationDisposition, uint FlagsAndAttributes, int hTemplateFile);

        /// <summary>
        /// Reads provided file.
        /// </summary>
        /// <param name="hFile">File pointer.</param>
        /// <param name="pBuffer">Buffer to read into.</param>
        /// <param name="NumberOfBytesToRead">Count of bytes to read.</param>
        /// <param name="pNumberOfBytesRead">Count of bytes to read.</param>
        /// <param name="Overlapped">Overlapped value.</param>
        /// <returns>True, if file was succesfully readed, otherwise false.</returns>
        [DllImport("kernel32", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        internal static extern unsafe bool ReadFile(IntPtr hFile, void* pBuffer, int NumberOfBytesToRead, int* pNumberOfBytesRead, int Overlapped);

        /// <summary>
        /// Closes provided file pointer.
        /// </summary>
        /// <param name="hObject">File pointer to close.</param>
        /// <returns>True, if file was closed successfully, otherwise false.</returns>
        [DllImport("kernel32", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        internal static extern unsafe bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// Opens provided file to read.
        /// </summary>
        /// <param name="fileName">Path to file to open.</param>
        /// <returns>True, if file was opened successfully, otherwise false.</returns>
        internal static IntPtr Open(string fileName)
        {
            return CreateFile(fileName, 0x80000000, 0x00, 0x00, 0x03, 0x00, 0x00);
        }

        /// <summary>
        /// Reads <see cref="byte"/> array from provided file.
        /// </summary>
        /// <param name="fileName">File name to read from.</param>
        /// <param name="lengthToRead">Count of bytes to read.</param>
        /// <returns>Array of <see cref="byte"/> values. </returns>
        /// <exception cref="FileLoadException" />
        /// <exception cref="FileNotFoundException" />
        /// <exception cref="IOException" />
        public static unsafe byte[] Read(string fileName, int lengthToRead)
        {
            IntPtr handle = Open(fileName);

            if (handle != IntPtr.Zero)
            {
                byte[] buffer = new byte[lengthToRead];

                fixed (byte* buf = buffer)
                {
                    if (ReadFile(handle, buf, lengthToRead, &lengthToRead, 0x00))
                    {
                        if (!CloseHandle(handle))
                            throw new IOException($"Failed to close file handle for '{fileName}'");

                        return buffer;
                    }
                }

                throw new FileLoadException($"Failed to read {lengthToRead} bytes from file '{fileName}'.");
            }

            throw new FileNotFoundException($"File '{fileName}' can't be found.");
        }

        /// <summary>
        /// Gets information ( array of <see cref="FileInfo"/> objects ), found by files <paramref name="mask"/> according to provided <see cref="SearchOption"/>.
        /// </summary>
        /// <param name="directory">Directory to search files in.</param>
        /// <param name="mask">Search pattern.</param>
        /// <param name="searchHow">Search option.</param>
        /// <returns>Array of <see cref="FileInfo"/> objects, containing information about files, that has been found.</returns>
        /// <exception cref="DirectoryNotFoundException" />
        public static FileInfo[] GetFiles(string directory, string mask, SearchOption searchHow)
        {
            if (Directory.Exists(directory))
                return new DirectoryInfo(directory).GetFiles(mask, searchHow);

            throw new DirectoryNotFoundException($"Directory '{directory}' can't be found.");
        }
    }
}