#region Copyright (c) 2000-2017 cjlc
/*
{+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++)
{                                                                   }
{     cjlc Cap control administrativo personal                      }
{                                                                   }
{     Copyrigth (c) 2000-2017 cjlc                                  }
{     Todos los derechos reservados                                 }
{                                                                   }
{*******************************************************************}
 */
/*
 * 
 * Cadena       Para convertir una cadena de números en su equivalente en letra.
 *              Falta hacerlo para otras lenguas... inglés... (JAAP)
 */
#endregion

using System;

namespace apl.Log
{
    using System.Globalization;

    /// <summary>
    /// 
    /// </summary>
    public class Cadena
    {
        /// <summary>
        /// Pasa el número a su equivalente en letra. Sólo la parte entera.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string CantidadLetra(decimal num)
        {
            double numeroOri = Convert.ToDouble(Math.Truncate(Math.Abs(num)));
            double numero;
            int pot;
            string letra = string.Empty;
            int mSig;

            string [ , ]nombres = { { "CERO ", "UN ",   "DOS ",  "TRES ",  "CUATRO ",  "CINCO ",  "SEIS ",      "SIETE ",      "OCHO ",      "NUEVE "}, 
                                    { "DIEZ ", "ONCE ", "DOCE ", "TRECE ", "CATORCE ", "QUINCE ", "DIECISEIS ", "DIECISIETE ", "DIECIOCHO ", "DIECINUEVE " }};
            string [, ]unidades = { { "DIEZ ",   "VEINTE ",     "TREINTA ",     "CUARENTA ",      "CINCUENTA ",  "SESENTA ",     "SETENTA ",     "OCHENTA ",     "NOVENTA " },
                                    { "CIENTO ", "DOSCIENTOS ", "TRESCIENTOS ", "CUATROCIENTOS ", "QUINIENTOS ", "SEISCIENTOS ", "SETECIENTOS ", "OCHOCIENTOS ", "NOVECIENTOS " }};
            string[] orden = { "", "MIL ", "MILLONES ", "BILLONES " };
            
            pot = Convert.ToInt32(Math.Truncate(Math.Log10(Convert.ToDouble(numeroOri))));

            numero = numeroOri;
            while (pot >= 0)
            {
                mSig = Convert.ToInt32(Math.Truncate(numero / (Math.Pow(10, pot))));
                numero = numero - Convert.ToDouble(mSig) * (Math.Pow(10, pot));


                if (mSig == 0)
                {
                    pot--;
                    continue;
                }

                if (pot % 3 == 0)
                {
                    letra += nombres[0, mSig];
                    letra += orden[pot / 3];
                }
                else if (pot % 4 == 0 || pot == 1)
                {
                    // El siguiente en 0,  como 20
                    if (Convert.ToInt32(Math.Truncate(numero / (Math.Pow(10, pot - 1)))) == 0)
                    {
                        letra += unidades[0, mSig - 1];
                        pot--;
                    }
                    // Como 17
                    else if (mSig == 1)
                    {
                        mSig = Convert.ToInt32(Math.Truncate(numero / (Math.Pow(10, --pot))));
                        numero = numero - Convert.ToDouble(mSig) * (Math.Pow(10, pot));

                        letra += nombres[1, mSig];
                    }
                    // Como 24
                    else
                    {
                        if (mSig == 2)
                            letra += "VEINTI ";
                        else
                        {
                            letra += unidades[0, mSig - 1];
                            letra += "Y ";
                        }
                    }

                    letra += orden[pot / 3];
                }
                else
                {
                    if (mSig == 1 && numero == 0)
                    {
                        letra += "CIEN ";
                    }
                    else
                    {
                        letra += unidades[pot == 1 ? 0 : 1, mSig - 1];
                    }
                    letra += orden[pot / 3];
                }
                pot--;
            }
            return letra;
        }

        /// <summary>
        /// Pasa a texto el número que se da v.gr.: 120  lo pasa a ciento veinte
        /// </summary>
        /// <param name="Cad">El numero como cadena</param>
        /// <returns></returns>
        public static string CantLetr(string Cad)
        {
            string []unidad = { "CERO",
                               "UN",
                               "DOS",
                               "TRES",
                               "CUATRO",
                               "CINCO",
                               "SEIS",
                               "SIETE",
		                       "OCHO",
                               "NUEVE" };

            string []diez =  {  "DIEZ",
                               "ONCE",
                               "DOCE",
                               "TRECE",
                               "CATORCE",
                               "QUINCE",
                               "DIECISEIS",
                               "DIECISIETE",
                               "DIECIOCHO",
                               "DIECINUEVE" };

            string []decena = { "DIEZ",
                               "VEINTE",
                               "TREINTA",
                               "CUARENTA",
                               "CINCUENTA",
                               "SESENTA",
		                       "SETENTA",
                               "OCHENTA",
                               "NOVENTA"  };

            string []centena = { "CIENTO",
                                "DOSCIENTOS",
                                "TRESCIENTOS",
                                "CUATROCIENTOS",
                                "QUINIENTOS",
                                "SEISCIENTOS",
                                "SETECIENTOS",
                                "OCHOCIENTOS",
                                "NOVECIENTOS" };

            string []orden = { " ",
                                " MIL ",
                                " MILLONES ",
                                " BILLONES " };

            int clenc;
            int i = 0;
            string aux = string.Empty;
            int chr;
            int unid;
            string lcad = string.Empty;
            NumberFormatInfo myInv = NumberFormatInfo.CurrentInfo;
            

            // Copiamos los caracteres validos
            //
            foreach (char c in Cad)
            {
                if (char.IsDigit(c))
                    aux += c.ToString();
                if (c.ToString() == myInv.CurrencyDecimalSeparator)
                    break;
            }

            clenc = aux.Length;
            while (i < clenc)
            {
                chr = Convert.ToInt16(aux[i]) - 48;   // valor del '0'
 
                // Unidades
                //
                if ((clenc-i-1)%3 == 0)
                {
                    // Por ejemplo si viene: 9200, chr va traer 0.
                    //
                    if (chr != 0)
                    {
                        lcad += unidad[chr];
                        unid = (clenc-i-1)/3;

                        // Caso especial de unidades del orden 10^6
                        // May 2002
                        if (chr == 1 && unid  ==  2)
                            lcad += " MILLON ";
                        else
                            lcad += orden[unid];
                    }
                    else if (clenc == 1)
                    {
                        lcad += unidad[chr];
	                }
                }


                // Decenas
                //
                else if ((clenc-i-1)%3  ==  1)
                {
                    if (chr == 1)
                    {
                        i++;
                        chr = aux[i];
	                    chr = chr - '0';

                        lcad += diez[chr];

                        unid = (clenc-i-1)/3;
                        lcad += orden[unid];
                    }
                    else if (chr == 2)
                    {
                        i++;
                        chr = aux[i];
                        chr = chr - '0';

                        if ( chr == 0 )
                            lcad += "VEINTE";
                        else
                        {
                            lcad += "VEINTI";
                            lcad += unidad[chr];
                        }
                        unid = (clenc-i-1)/3;
                        lcad += orden[unid];
                    }
                    // Por ejemplo si viene: 9200, chr va traer: 0
                    else if (chr != 0)
                    {
                        lcad += decena[chr-1];

                        // si es diferente de algo como: 30, 40, 50, ...
                        if (aux[i+1] != '0')
                            lcad += " Y ";
                        else
                        {
                            unid = (clenc-i-1)/3;
                            lcad += orden[unid];
                            i++;
                        }
                    }
                }

                // Centenas
                // Si es la posicion de las centenas y hay digito
                //
                else if ((clenc-i-1)%3 == 2 && chr != 0)
                {
                    // Jun 2001 si viene '100' se pone 'cien' y no 'ciento'
                    //
                    if ((i + 2) < clenc
                        && aux[i] == '1' && aux[i + 1] == '0' && aux[i + 2] == '0')
                    {
                        lcad += "CIEN";

                        unid = (clenc - i - 1) / 3;
                        lcad += orden[unid];
                    }
                    else
                    {
                        if ((i + 2) < clenc && aux[i + 1] == '0' && aux[i + 2] == '0')
                        {
                            lcad += centena[chr - 1];
                            unid = (clenc - i - 1) / 3;
                            lcad += orden[unid];
                        }
                        else
                            lcad += centena[chr - 1];
                    }

                    lcad += " ";
                }

                i++;
            }

            // May 2002, para saber si va la palabra 'DE'
            if ( clenc > 6 )
            {
                for (i = clenc-1; i > 0 && aux [i] == '0'; --i)
                    ;
                if ( (clenc-i) > 6 )
                    lcad += " DE ";
            }

            return lcad;
        }
        
        /// <summary>
        /// Convirte una cantidad "1 234" a su equivalente en letra en lenguaje inglés: one thousand two hundred thirty four
        /// </summary>
        /// <param name="Cad">Numero como cadena</param>
        /// <returns></returns>
        public static string CantLetrDlls(string Cad)
        {
            string[] unidad = {"ZERO",
                               "ONE",
                               "TWO",
                               "THREE",
                               "FOUR",
                               "FIVE",
                               "SIX",
                               "SEVEN",
		                       "EIGHT",
                               "NINE" };

            string[] diez =  { "TEN",
                               "ELEVEN",
                               "TWELVE",
                               "THIRTEEN",
                               "FOURTEEN",
                               "FIFTEEN",
                               "SIXTEEN",
                               "SEVENTEEN",
                               "EIGHTEEN",
                               "NINETEEN" };

            string[] decena = {"TEN",
                               "TWENTY",
                               "THIRTY",
                               "FORTY",
                               "FIFTY",
                               "SIXTY",
		                       "SEVENTY",
                               "EIGHTY",
                               "NINETY"  };

            string[] centena = {"ONE HUNDRED",
                                "TWO HUNDRED",
                                "THREE HUNDRED",
                                "FOUR HUNDRED",
                                "FIVE HUNDRED",
                                "SIX HUNDRED",
                                "SEVEN HUNDRED",
                                "EIGHT HUNDRED",
                                "NINE HUNDRED" };

            string[] orden = { " ",
                                " THOUSAND ",
                                " MILLIONS ",
                                " BILLIONS " };

            int clenc;
            int i = 0;
            string aux = "";
            int chr;
            int unid;
            string lcad = "";
            NumberFormatInfo myInv = NumberFormatInfo.CurrentInfo;


            // Copiamos los caracteres validos
            //
            foreach (char c in Cad)
            {
                if (char.IsDigit(c))
                    aux += c.ToString();
                if (c.ToString() == myInv.CurrencyDecimalSeparator)
                    break;
            }

            clenc = aux.Length;
            while (i < clenc)
            {
                chr = Convert.ToInt16(aux[i]) - 48;   // valor del '0'

                // Unidades
                //
                if ((clenc - i - 1) % 3 == 0)
                {
                    // Por ejemplo si viene: 9200, chr va traer 0.
                    //
                    if (chr != 0)
                    {
                        lcad += unidad[chr];
                        unid = (clenc - i - 1) / 3;

                        // Caso especial de unidades del orden 10^6
                        // May 2002
                        if (chr == 1 && unid == 2)
                            lcad += " MILLION ";
                        else
                            lcad += orden[unid];
                    }
                    else if (clenc == 1)
                    {
                        lcad += unidad[chr];
                    }
                }


                // Decenas
                //
                else if ((clenc - i - 1) % 3 == 1)
                {
                    if (chr == 1)
                    {
                        i++;
                        chr = aux[i];
                        chr = chr - '0';

                        lcad += diez[chr];

                        unid = (clenc - i - 1) / 3;
                        lcad += orden[unid];
                    }
                    else if (chr == 2)
                    {
                        i++;
                        chr = aux[i];
                        chr = chr - '0';

                        if (chr == 0)
                            lcad += "TWENTY";
                        else
                        {
                            lcad += "TWENTY ";
                            lcad += unidad[chr];
                        }
                        unid = (clenc - i - 1) / 3;
                        lcad += orden[unid];
                    }
                    // Por ejemplo si viene: 9200, chr va traer: 0
                    else if (chr != 0)
                    {
                        lcad += decena[chr - 1];

                        // si es diferente de algo como: 30, 40, 50, ...
                        if (aux[i + 1] != '0')
                            lcad += " ";
                        else
                        {
                            unid = (clenc - i - 1) / 3;
                            lcad += orden[unid];
                            i++;
                        }
                    }
                }

                // Centenas
                // Si es la posicion de las centenas y hay digito
                //
                else if ((clenc - i - 1) % 3 == 2 && chr != 0)
                {
                    // Jun 2001 si viene '100' se pone 'cien' y no 'ciento'
                    //
                    if ((i + 2) < clenc
                        && aux[i] == '1' && aux[i + 1] == '0' && aux[i + 2] == '0')
                    {
                        lcad += "HOUNDRED";

                        unid = (clenc - i - 1) / 3;
                        lcad += orden[unid];
                    }
                    else
                        lcad += centena[chr - 1];

                    lcad += " ";
                }

                i++;
            }

            // May 2002, para saber si va la palabra 'DE'
            if (clenc > 6)
            {
                for (i = clenc - 1; i > 0 && aux[i] == '0'; --i)
                    ;
                if ((clenc - i) > 6)
                    lcad += " ";
            }

            return lcad;
        }


        //#region Cantidad terminacion moneda
        /// <summary>
        /// Regresa la terminación adecuada dependiendo de la moneda.
        /// Si el monto es 120.78, retorna: PESOS 78/100 M.N.
        /// </summary>
        /// <param name="_monto"></param>
        /// <param name="_nombres"></param>
        /// <param name="_terminacion"></param>
        /// <returns></returns>
        public static string Terminacion(decimal _monto, string _nombres, string _terminacion)
        {
            char[] splitter = { '/' };
            string[] arrtoks = null;
            string text = string.Empty;
            decimal cantidad = _monto;


            if (!string.IsNullOrEmpty(_nombres))
                arrtoks = _nombres.Split(splitter);

            if (arrtoks != null && arrtoks.Length > 0)
            {
                if (_monto == 1.0m)
                    text = arrtoks[0];
                else
                    text = arrtoks.Length > 1 ? arrtoks[1] : string.Empty;
            }

            cantidad = cantidad - Math.Truncate(cantidad);
            cantidad = Math.Truncate(cantidad * 100);


            // text += string.Format(" {0}/100 ", TermPad() ? cantidad.ToString().PadLeft(2,'0') : cantidad.ToString());
            text += string.Format(" {0}/100 ", cantidad.ToString().PadLeft(2, '0'));
            text += _terminacion;
            return text;
        }

        /// <summary>
        /// Para saber si la cadena representa un número
        /// </summary>
        /// <param name="cadena">Cadena a verificar</param>
        /// <returns></returns>
        public static bool IsNumber(string cadena)
        {
            foreach (char c in cadena)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return cadena.Length > 0;
        }
    }
}
