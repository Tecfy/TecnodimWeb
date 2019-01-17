using System;

public static class HelperFormatCnpjCpf
{
    public static string FormatCNPJ(string CNPJ)
    {
        CNPJ = DeleteFormat(CNPJ);

        return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
    }

    public static string FormatCPF(string CPF)
    {
        if (!string.IsNullOrEmpty(CPF))
        {
            CPF = DeleteFormat(CPF);

            return Convert.ToUInt64(CPF).ToString(@"000\.000\.000\-00");
        }
        else
        {
            return null;
        }
    }

    public static string DeleteFormat(string Codigo)
    {
        return Codigo.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
    }
}