using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tof.Uzorci.Decorator
{
    /// <summary>
    /// 'ConcreteDecorator' klasa
    /// </summary>

    public class BrojLinijaBuffer : StreamWriter
    {
        private readonly string[] _meduspremnik;
        private StreamWriter _baseWriter;

        public BrojLinijaBuffer(StreamWriter sw, int brojLinija) : base(sw.BaseStream)
        {
            if(brojLinija <= 0)
            {
                throw new Iznimke.NeispravnaVelicinaMeduspremnika();
            }
            _meduspremnik = new string[brojLinija];
            _baseWriter = sw;
        }

        public void Write(string[] data)
        {            
            int start = 0;

            while (start != data.Length)
            {
                var len = _meduspremnik.Length;
                if(data.Length - start < len)
                {
                    len = data.Length - start;
                }
                Array.Copy(data, start, _meduspremnik, 0, len);
                for (int i = 0; i < len; i++)
                {
                    _baseWriter.WriteLine(_meduspremnik[i]);
                }
                start += len;
            }            
        }
    }
}
