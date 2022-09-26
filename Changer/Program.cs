using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Changer
{
    class Program
    {
        public static string Xor(string input, string stringKey)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {

                int utf = (int)(input[i] ^ stringKey[i % stringKey.Length]);
                stringBuilder.AppendFormat("{0}", char.ConvertFromUtf32(utf));
            }
            return stringBuilder.ToString();
        }
        private static string FromBase64(string base64str)
        {
            return BytesToStringConverted(Convert.FromBase64CharArray(base64str.ToCharArray(), 0, base64str.Length));
        }
        private static string BytesToStringConverted(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
        public static string Read(string b64, string stringKey)
        {
            string result;
            try
            {
                if (string.IsNullOrWhiteSpace(b64))
                {
                    result = string.Empty;
                }
                else
                {
                    result = FromBase64(Xor(FromBase64(b64), stringKey));
                }
            }
            catch
            {
                result = b64;
            }
            return result;
        }

        private static string toBase64(string base64str)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(base64str), 0, base64str.Length);
        }

        public static string ReadV2(string b64, string stringKey)
        {
            string result;
            try
            {
                if (string.IsNullOrWhiteSpace(b64))
                {
                    result = string.Empty;
                }
                else
                {
                    result = toBase64(Xor(toBase64(b64), stringKey));
                }
            }
            catch
            {
                result = b64;
            }
            return result;
        }
        static void Main(string[] args)
        {
            ModuleContext modCtx = ModuleDef.CreateModuleContext();
            ModuleDefMD module = ModuleDefMD.Load(@".\GeneratedStub.exe", modCtx);

            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (method.HasBody == false)
                        continue;
                    if (method.Body.HasInstructions)
                    {
                        for (int i = 0; i < method.Body.Instructions.Count; i++)
                        {
                            if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr && method.Body.Instructions[i].Operand.ToString().Contains("BTsEVj8iIl0qGQ1cIR9HfSIrCUcFLlJb"))
                            {
                                Console.WriteLine(method.Body.Instructions[i].OpCode);
                                    method.Body.Instructions[i].Operand = ReadV2(args[0], "Hoofs");

                            }
                        }
                    }
                }
            }
            module.Write("./GeneratedStub2.exe");
           // Console.WriteLine(Read("HjwOHigDKxA9OlwdKzAsGyonHhAqDgMe", "Sheddings"));
        }
    }
}
