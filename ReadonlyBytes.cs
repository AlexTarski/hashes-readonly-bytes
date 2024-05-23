using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hashes
{
    public class ReadonlyBytes
    {
        readonly byte[] bytesArray;
        private int hash;
        private bool hashIsCalculated = false;

        public ReadonlyBytes(params byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            List<byte> result = new List<byte>();
            foreach (byte b in bytes)
            {
                result.Add(b);
            }
            bytesArray = result.ToArray();
        }


        public override int GetHashCode()
        {
            unchecked
            {
                if (hashIsCalculated)
                {
                    return this.hash;
                }
                this.hash = unchecked((int)2166136261);

                foreach (byte b in bytesArray)
                {
                    this.hash ^= b;
                    this.hash *= 16777619;
                }
                hashIsCalculated = true;
                return this.hash;
            }
        }

        public int Length => bytesArray.Length;

        internal byte this[int index]
        {
            get
            {
                if (index < 0 || index >= bytesArray.Length) throw new IndexOutOfRangeException();
                return bytesArray[index];
            }
            set
            {
                if (index < 0 || index >=bytesArray.Length) throw new IndexOutOfRangeException();
                bytesArray[index] = value;
            }
        }

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }
            else if (obj.GetType() != typeof(ReadonlyBytes))
            {
                return false;
            }
            else
            {
                ReadonlyBytes temp = (ReadonlyBytes)obj;
                if(temp.Length > this.Length || temp.Length < this.Length)
                {
                    return false;
                }
                else
                {
                    return temp.GetHashCode() == this.GetHashCode();
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            foreach (byte b in bytesArray)
            {
                sb.Append($"{b.ToString()}, ");
            }
            if (bytesArray.Length > 0)
            {
                sb.Remove(sb.Length - 2, 2);
            }
            sb.Append(']');
            return sb.ToString();
        }

        public IEnumerator<byte> GetEnumerator() => new ByteEnumerator(bytesArray);
    }

    class ByteEnumerator : IEnumerator<byte>
    {
        byte[] bytes;
        int position = -1;
        public ByteEnumerator(byte[] bytes) => this.bytes = bytes;
        public byte Current
        {
            get
            {
                if (position == -1 || position >= bytes.Length)
                    throw new ArgumentOutOfRangeException(nameof(position), "position out of range");
                return bytes[position];
            }
        }
        object IEnumerator.Current => Current;
        public bool MoveNext()
        {
            if (position < bytes.Length - 1)
            {
                position++;
                return true;
            }
            else
                return false;
        }

        public void Reset() => position = -1;
        public void Dispose() { }
    }
}