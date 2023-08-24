using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Search
{
    internal class FileToCharStream : FileStream
    {
        public Bom Bom { get; private set; }
        public int BytesRead { get; private set; }

        #region ctors

        public FileToCharStream(SafeFileHandle handle, FileAccess access) : base(handle, access)
        {
            Bom = this.Bom();
        }

        public FileToCharStream(IntPtr handle, FileAccess access) : base(handle, access)
        {
            Bom = this.Bom();
        }

        public FileToCharStream(string path, FileMode mode) : base(path, mode)
        {
            Bom = this.Bom();
        }

        public FileToCharStream(string path, FileStreamOptions options) : base(path, options)
        {
            Bom = this.Bom();
        }

        public FileToCharStream(SafeFileHandle handle, FileAccess access, int bufferSize) : base(handle, access, bufferSize)
        {
            Bom = this.Bom();
        }

        public FileToCharStream(IntPtr handle, FileAccess access, bool ownsHandle) : base(handle, access, ownsHandle)
        {
            Bom = this.Bom();
        }

        public FileToCharStream(string path, FileMode mode, FileAccess access) : base(path, mode, access)
        {
            Bom = this.Bom();
        }

        public FileToCharStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) : base(handle, access, bufferSize, isAsync)
        {
            Bom = this.Bom();
        }

        public FileToCharStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize) : base(handle, access, ownsHandle, bufferSize)
        {
            Bom = this.Bom();
        }

        public FileToCharStream(string path, FileMode mode, FileAccess access, FileShare share) : base(path, mode, access, share)
        {
            Bom = this.Bom();
        }

        public FileToCharStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync) : base(handle, access, ownsHandle, bufferSize, isAsync)
        {
            Bom = this.Bom();
        }

        public FileToCharStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize) : base(path, mode, access, share, bufferSize)
        {
            Bom = this.Bom();
        }

        public FileToCharStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync) : base(path, mode, access, share, bufferSize, useAsync)
        {
            Bom = this.Bom();
        }

        public FileToCharStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options) : base(path, mode, access, share, bufferSize, options)
        {
            Bom = this.Bom();
        }

        #endregion

        public override long Seek(long offset, SeekOrigin origin)
            => base.Seek(offset + Bom.Length, origin) - Bom.Length;

        public override int Read(byte[] buffer, int offset, int count)
        {
            BytesRead = base.Read(buffer, offset, count);
            Eof = BytesRead != count;
            return BytesRead;
        }

        public char[] ReadToCharArray(byte[] buffer, int offset, int count)
        {
            Read(buffer, offset, count);
            return buffer.ToCharArray(BytesRead, Bom.Encoding);
        }

        public bool Eof { get; set; }
    }
}
