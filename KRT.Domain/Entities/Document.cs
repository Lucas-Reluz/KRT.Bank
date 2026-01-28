using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KRT.Domain.Entities
{
    public class Document
    {
        public string Value { get; }


        public Document(string value)
        {
            var docvValid = ValidateLength(value);
            if (docvValid)
                Value = value;
        }

        public static bool ValidateCpf(string cpf)
        {
            cpf = Regex.Replace(cpf, @"\D", "");
            if (cpf.Length != 11) return false;

            if (new string(cpf[0], cpf.Length) == cpf) return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int digito = resto < 2 ? 0 : 11 - resto;
            string resultado = tempCpf + digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(resultado[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            digito = resto < 2 ? 0 : 11 - resto;
            resultado += digito;

            return cpf.EndsWith(resultado.Substring(9, 2));
        }

        public static bool ValidateCnpj(string cnpj)
        {
            cnpj = Regex.Replace(cnpj, @"\D", "");
            if (cnpj.Length != 14) return false;

            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int digito = resto < 2 ? 0 : 11 - resto;
            string resultado = tempCnpj + digito;

            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(resultado[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            digito = resto < 2 ? 0 : 11 - resto;
            resultado += digito;

            return cnpj.EndsWith(resultado.Substring(12, 2));
        }

        public static bool ValidateLength(string document)
        {
            string onlyNumbers = Regex.Replace(document ?? "", @"\D", "");

            if (onlyNumbers.Length == 11)
                return ValidateCpf(onlyNumbers);

            if (onlyNumbers.Length == 14)
                return ValidateCnpj(onlyNumbers);

            return false;
        }
    }
}
