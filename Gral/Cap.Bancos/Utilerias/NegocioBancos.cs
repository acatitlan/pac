using System;
using System.Collections;
using System.Collections.Specialized;
using apl.Log;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using Cap.Bancos.BusinessObjects;
using Cap.Generales.BusinessObjects.General;

namespace Cap.Bancos.Utilerias
{
    public class NegocioBancos
    {
        static public short TantosErr;

        static public string MsgError;
        // Puede ir en la configuración del sistema
        public const string ClvCptIni = "A.SLDI";

        //#region + Graba cuenta
        // ------------------------------------------------------------------
        // Actualizamos el catálogo de cuentas
        // Si la cuenta es de tipo crédito => se genera un
        // movimiento de tipo agenda.
        // May 2003, Revisar la rutina, pues ahora se puede grabar en una partida
        // lo de agenda.
        // Jun 2005, Si la cuenta tiene saldo inicial éste se graba a partir de un
        // movimiento a la cuenta. Es decir el saldo inicial se hace 0 y el final toma
        // el valor del inicial.
        // Usa una unit of work
        //
        public static bool GrabaCuenta(Bancaria _cuenta)
        {
            bool nuevaCuenta;
            decimal sldIni;


            try
            {
                if (!ValidaCuenta(_cuenta))
                    throw new Exception(MsgError);

                sldIni = 0.0m;
                nuevaCuenta = _cuenta.IsNewObject();

                if (nuevaCuenta && _cuenta.SaldoInicial != 0.0m)
                {
                    sldIni = _cuenta.SaldoInicial;
                    _cuenta.SaldoInicial = 0.0m;
                    _cuenta.SaldoFinal = 0.0m;
                }
                // Ene 2013 Commit
                //_cuenta.Save();

                #region Saldo inicial al dar de alta la cuenta
                if (sldIni != 0.0m)
                {
                    ConceptoB concepto;
                    MovimientoB movimiento = new MovimientoB(_cuenta.Session);


                    CriteriaOperator criteria = CriteriaOperator.Parse(string.Format("Clave = '{0}'", ClvCptIni) /*'A.SLDI'"*/);
                    concepto = _cuenta.Session.FindObject(typeof(ConceptoB), criteria) as ConceptoB;


                    movimiento.Cuenta = _cuenta;
                    movimiento.Concepto = concepto;
                    movimiento.Moneda = _cuenta.Moneda;
                    movimiento.TipoCambio = 1.0m;
                    movimiento.Monto = sldIni;
                    movimiento.FechaAlta = _cuenta.FechaAlta;
                    // Mar 2013, lo pondremos al inicio de los tiempos
                    // Para que la opción de Saldo inicial tome en 
                    // cuenta este movimiento 
                    // movimiento.FechaAplicacion = _cuenta.FechaAlta;
                    movimiento.FechaAplicacion = new DateTime(1900, 1, 1);
                    movimiento.Notas = "Saldo inicial de apertura de cuenta";

                    GrabaMovimiento(movimiento);
                }
                #endregion

                if (nuevaCuenta && sldIni != 0.0m)
                {
                    _cuenta.SaldoInicial = sldIni;
                    // Ene 2013 Commit
                    // _cuenta.Save();
                }
                return true;
            }
            catch (Exception)
            {
                LogDebug.Escribe("No se pudo grabar la cuenta: Bancos");
                throw;
            }
        }

        static public bool ValidaCuenta(Bancaria cta)
        {
            short TantosErr = 0;
            StringCollection Msgs = new StringCollection();


            if (string.IsNullOrEmpty(cta.Clave))
                Msgs.Add("Hay que asignar una clave a la cuenta");

            // Tal vez hay que agregar que no se pueda modificar la moneda 
            // si la cuenta tiene movimientos con otra cuenta y tipo de cambio
            // diferente de 1
            if (cta.Moneda == null)
                Msgs.Add("Hay que asignar una moneda a la cuenta");

            if (cta.CuotaAnual < 0.0M)
                Msgs.Add("La cuota anual deber ser mayor o igual a cero.");

            if (cta.Tipo == ECuentaTipo.Credito)
            {
                if (cta.DiaCorte < 1 || cta.DiaCorte > 31)
                    Msgs.Add("El dia de corte debe estar entre 1 y 31");

                if (cta.DiaPago < 1 || cta.DiaPago > 31)
                    Msgs.Add("El día de pago debe estar entre 1 y 31");

                if (cta.SaldoLimite < 0)
                    Msgs.Add("El límite de crédito no puede ser negativo.");
                /*
                if (/ *!cta.valSldLimite()* /cta.SaldoLimite < 0.0M)
                    Msgs.Add("El saldo límite no puede ser negativo");*/
            }

            else if (cta.Tipo == ECuentaTipo.Maestra)
            {
                if (cta.NumeroCheque <= 0)
                    Msgs.Add("El número de cheque debe ser mayor que 0");
            }


            if (cta.SaldoInicial < 0)
                Msgs.Add("El saldo inicial debe ser mayor o igual a cero.");

            if (cta.PorcentajeInteres < 0.0 || cta.PorcentajeInteres > 1.0)
                Msgs.Add("El porcentaje de interés debe estar entre 0 y 100");

            if (cta.Saldominimo < 0.0M)
                Msgs.Add("El saldo mínimo no puede ser negativo");

            TantosErr = (short)Msgs.Count;
            MsgError = TantosErr > 0 ? Msgs[0] : string.Empty;

            return TantosErr == 0;
        }

        static public bool EsCredito(Bancaria cnt)
        {
            return cnt.Tipo == ECuentaTipo.Credito;
        }

        //#region + Status baja
        static public void Baja(Bancaria cnt)
        {
            cnt.Status = StatusTipoA.Baja;
        }

        //#region + Status Activa
        static public void Activa(Bancaria cnt)
        {
            cnt.Status = StatusTipoA.Activa;
        }




        static public bool GrabaTransfer(Transferencia trans)
        {
            bool ok = true;
            MovimientoB orig = new MovimientoB(trans.Session);
            MovimientoB dest = new MovimientoB(trans.Session);

            orig.Cuenta = trans.CtaOrigen;
            dest.Cuenta = trans.CtaDestino;
            orig.Monto = trans.Monto;
            dest.Monto = trans.Monto;
            orig.FechaAlta = DateTime.Now;
            dest.FechaAlta = DateTime.Now;
            orig.FechaAplicacion = trans.FecApli;
            dest.FechaAplicacion = trans.FecApli;
            orig.Notas = trans.Notas;
            dest.Notas = trans.Notas;
            orig.Concepto = trans.Session.FindObject<ConceptoB>(new BinaryOperator("Clave", "C.TR"));
            dest.Concepto = trans.Session.FindObject<ConceptoB>(new BinaryOperator("Clave", "A.TR"));
            orig.Rlcnd = dest;
            dest.Rlcnd = orig;

            ok = GrabaMovimiento(orig);
            ok = ok && GrabaMovimiento(dest);
            return ok;
        }

        static public bool GrabaMovimiento(MovimientoB movimiento)
        {
            return GrabaMovimiento(movimiento, false);
        }

        static public bool GrabaMovimiento(MovimientoB _movimiento, bool _recupera)
        {
            return GrabaMovimiento(_movimiento, _recupera, true);
        }

        // ------------------------------------------------------------------
        // Grabamos el movimiento.
        // Para el caso de las tarjetas de crédito hacemos algo más
        // Si la fecha de aplicacion es posterior a la actual entonces es un movimiento en transito.
        // Falta ver qué hongo con dejar subir las excepciones o no...
        //
        static public bool GrabaMovimiento(MovimientoB movimiento, bool recupera, bool grabaEven)
        {
            bool opc = true;
            bool alta = true;
            bool eraTransito = false;

            try
            {
                if (!ValidaMovimiento(movimiento))
                    throw new Exception(MsgError);


                #region Graba el movimiento
                if (movimiento.IsNewObject())
                {
                    if (movimiento.FechaAplicacion.Date > DateTime.Today.Date)
                        Transito(movimiento);
                }
                else if (!recupera)
                {
                    if (EsTransito(movimiento) && movimiento.FechaAplicacion.Date <= DateTime.Today.Date)
                    {
                        Alta(movimiento);
                        eraTransito = true;
                    }

                    if (movimiento.Cuenta != null && movimiento.Cuenta.Clave != movimiento.CuentaA.Clave)
                        throw new Exception("No se puede cambiar la cuenta de un movimiento registrado !");
                    if (movimiento.Concepto.Tipo != movimiento.ConceptoA.Tipo)
                        throw new Exception("No se puede cambiar el Tipo del concepto de un movimiento registrado !");
                    if (movimiento.StatusA == MovimientoStatus.Cancelado)
                        throw new Exception("No se puede modificar un movimiento cancelado !");
                    if (movimiento.Tipo != EMovimientoTipo.Gasto && movimiento.TipoA == EMovimientoTipo.Gasto)
                        throw new Exception("No se puede modificar un movimiento Gasto a otro tipo !");

                    if (movimiento.Tipo != EMovimientoTipo.Gasto)
                    {
                        if (movimiento.Monto != movimiento.MontoA)
                            throw new Exception("No se puede cambiar el Monto de un movimiento registrado !");
                    }
                }
                opc = movimiento.IsNewObject();
                // Ene 2013 Commit
                // movimiento.Save();
                #endregion

                #region Actualiza cuentas
                // Abr 2003, Actualizamos lo demas si el movimiento no es cancelado
                // Vemos si la cuenta es de tipo crédito
                //
                if (!EstaCancelado(movimiento) && movimiento.Tipo != EMovimientoTipo.Gasto)
                {
                    if (opc == alta || recupera)
                    {
                        // si es un movimiento en tránsito
                        if (EsTransito(movimiento))
                            movimiento.Cuenta.SaldoTransito += movimiento.MontoAplicar;
                        else
                        {
                            if (EsCredito(movimiento.Cuenta))
                            {
                                if (EsSaldoIni(movimiento.Concepto))
                                    movimiento.Cuenta.SaldoFinal += movimiento.MontoAplicar;
                                else
                                {
                                    movimiento.Cuenta.SaldoFinal += (EsCargo(movimiento)) ? movimiento.MontoAplicar : -movimiento.MontoAplicar;
                                }
                            }
                            else
                                movimiento.Cuenta.SaldoFinal += (EsCargo(movimiento)) ? -movimiento.MontoAplicar : movimiento.MontoAplicar;
                        }
                        if (EsCheque(movimiento) && !recupera)
                            movimiento.Cuenta.NumeroCheque = movimiento.Cuenta.NumeroCheque + 1;

                        movimiento.Cuenta.FechaUltimoMov = movimiento.FechaAlta;

                        movimiento.Cuenta.SaldoPromedio = SaldoPromedio(movimiento, movimiento.FechaAplicacion, movimiento.Cuenta);

                    }
                    else if (eraTransito)
                    {
                        // Abr 2003, por el momento suponemos que venimos de un status en
                        //           transito.
                        movimiento.Cuenta.SaldoTransito -= movimiento.MontoAplicar;

                        if (/*movimiento.Cuenta.*/EsCredito(movimiento.Cuenta))
                        {
                            movimiento.Cuenta.SaldoFinal += (/*movimiento.*/EsCargo(movimiento))
                                ? movimiento.MontoAplicar : -movimiento.MontoAplicar;
                        }
                        else
                            movimiento.Cuenta.SaldoFinal += (/*movimiento.*/EsCargo(movimiento))
                                ? -movimiento.MontoAplicar : movimiento.MontoAplicar;

                        movimiento.Cuenta.FechaUltimoMov = movimiento.FechaAlta;
                        //
                        // Ene 2013 Commit
                        // movimiento.Cuenta.Save();
                    }


                    #region Agenda, pendiente
                    // La cuenta es de tipo crédito, y el movimiento se está
                    // dando de alta
                    //
                    /* TODO: Parece que esto ya no se usa
                    if (Opc == Movimientos->ABC_ALTA  &&  Movimientos->TipCta == 0)
                    {*/
                    // La cuenta es de credito entonces hay que actualizar
                    // el monto en la agenda
                    //
                    /*
                    Agenda*   pAgen;

                    pAgen = new Agenda();*/
                    // Revisar esto
                    // ukdateAgen(Movimientos);
                    // delete pAgen;   pAgen = NULL;
                    // }
                    #endregion
                }
                #endregion

                if (movimiento.Tipo != EMovimientoTipo.Gasto)
                {
                    #region Actualiza marcas
                    updateMarca(opc, movimiento);
                    #endregion

                    #region Grabar agenda
                    GrabaAgenda(movimiento);
                    #endregion
                }
            }
            catch (Exception)
            {
                LogDebug.Escribe("No se pudo grabar el movimiento: Bancos");
                throw;
            }
            return true;
        }

        static public bool ValidaMovimiento(MovimientoB mov)
        {
            StringCollection Msgs = new StringCollection();


            Msgs.Clear();
            if (mov.Tipo != EMovimientoTipo.Gasto && mov.Cuenta == null)
                Msgs.Add("Debe asignar una cuenta.");

            if (mov.Cuenta != null && EsBaja(mov.Cuenta))
                Msgs.Add("La cuenta esta dada de baja !");

            if (mov.StatusA == MovimientoStatus.Cancelado)
                Msgs.Add("El Movimiento cancelado no se puede modificar !");

            if (mov.Concepto == null)
                Msgs.Add("Debe asignar un concepto al movimiento");
            else if (!mov.Concepto.EsAbono() && !mov.Concepto.EsCargo())
                Msgs.Add("El concepto debe ser cargo o abono");

            // Ene 2013 No recuerdo porque en el dlg no hago esta val
            /* A veces el rendimiento de la AFORE es negativo, se paso la validación != 0 a la clase 2015
            if (mov.Monto <= 0)
                Msgs.Add("El monto debe ser mayor que cero.");
            else
            {*/
            // Mas que el objeto sea nuevo, es que todavia no se aplique el monto
            if (mov.Status == MovimientoStatus.Firme && mov.StatusA != MovimientoStatus.Firme)
            {
                if (mov.Concepto != null && mov.Concepto.EsCargo())
                {
                    if (mov.Cuenta != null && EsCredito(mov.Cuenta))
                    {
                        if (!mov.Cuenta.VerificaSaldo && (mov.Cuenta.SaldoLimite > 0 && mov.Cuenta.SaldoLimite < mov.Cuenta.SaldoFinal + mov.MontoAplicar))
                            Msgs.Add("El monto es mayor que el saldo límite de la cuenta");
                    }
                    else
                    {
                        if (mov.Cuenta != null && !mov.Cuenta.VerificaSaldo && mov.Cuenta.SaldoFinal < mov.MontoAplicar)
                            Msgs.Add("El monto es mayor que el saldo de la cuenta");
                    }
                }
            }
            // }

            if (mov.Moneda == null && mov.Tipo != EMovimientoTipo.Gasto)
                Msgs.Add("Debe asignar una moneda");
            if (mov.TipoCambio <= 0.0m)
                Msgs.Add("El tipo de cambio debe ser mayor que cero.");

            if (mov.Tipo == EMovimientoTipo.Cheque)
            {
                if (mov.Folio <= 0)
                    Msgs.Add("Debe proporcionar un número de cheque mayor que cero.");
                if (mov.Concepto != null && !mov.Concepto.EsCargo())
                    Msgs.Add("Tipo cheque sólo con conceptos CARGO ");
                if (mov.Cuenta != null && !EsMaestra(mov.Cuenta))
                    Msgs.Add("El cheque sólo con cuenta Maestra");
                if (string.IsNullOrEmpty(mov.Notas))
                    Msgs.Add("Debe asignar un beneficiario.");
            }

            TantosErr = (short)Msgs.Count;
            MsgError = TantosErr > 0 ? Msgs[0] : string.Empty;

            return TantosErr == 0;
        }

        //#region + set Transito status
        static public void Transito(MovimientoB mov)
        {
            mov.Status = MovimientoStatus.Transito;
        }

        static public bool EsTransito(MovimientoB mov)
        {
            return mov.Status == MovimientoStatus.Transito;
        }

        static public void Alta(MovimientoB mov)
        {
            mov.Status = MovimientoStatus.Firme;
        }

        static public bool EstaCancelado(MovimientoB mov)
        {
            return mov.Status == MovimientoStatus.Cancelado;
        }

        static public bool EsCargo(MovimientoB mov)
        {
            return mov.ConceptoTipo == EConceptoTipo.Cargo;
        }

        /// <summary>
        /// Antes se veia el folio, pero ahora lo vemos en el tipo
        /// </summary>
        static public bool EsCheque(MovimientoB mov)
        {
            return mov.Tipo == EMovimientoTipo.Cheque;
        }

        // Por el momento suponemos que se ejecuta justo cuando se va a guardar un movimiento
        // y por esta razón como todavia no se hace comit, entonces el xpview no tiene este 
        // ultimo movimiento....
        static private decimal SaldoPromedio(MovimientoB _movimiento, DateTime _fecha, Bancaria _cuenta)
        {
            try
            {
                decimal sp = 0.0m;
                decimal[] saldo;
                decimal monto;
                int i = 0, days;
                XPView xpViewMovs;
                DateTime auxfec = Fecha.FechaFinal(_fecha.Month, _fecha.Year);

                days = auxfec.Day;

                saldo = new decimal[days];
                try
                {
                    saldo[0] = SldIni(_fecha, _cuenta);
                }
                catch (Exception)
                {
                    saldo[0] = 0.0m;
                }
                for (i = 1; i < days; i++)
                    saldo[i] = saldo[0];
                if (_movimiento != null)
                {
                    i = _movimiento.FechaAplicacion.Day - 1;
                    for (; i < days; i++)
                        saldo[i] += _movimiento.ConceptoTipo == EConceptoTipo.Abono ? _movimiento.Monto : -_movimiento.Monto;
                }

                xpViewMovs = new XPView();

                ((System.ComponentModel.ISupportInitialize)(xpViewMovs)).BeginInit();
                xpViewMovs.ObjectType = typeof(MovimientoB);
                xpViewMovs.Properties.AddRange(new ViewProperty[] {
                    new ViewProperty("FechaAplicacion", SortDirection.None, "[FechaAplicacion]", false, true),
                    new ViewProperty("Monto", SortDirection.None, "[Monto]", false, true),
                    new ViewProperty("ConceptoTipo", SortDirection.None, "[ConceptoTipo]", false, true)
                });

                BinaryOperator[] operands = new BinaryOperator[5];
                // DateTime auxfin = Fecha.FechaFinal(_fecha.Month);

                operands[0] = new BinaryOperator("Cuenta", XpoDefault.Session.FindObject(typeof(Bancaria), new BinaryOperator("Clave", /*_movimiento.Cuenta.Clave*/_cuenta.Clave)));
                operands[1] = new BinaryOperator("FechaAplicacion", Fecha.FechaInicial(_fecha.Month), BinaryOperatorType.GreaterOrEqual);
                operands[2] = new BinaryOperator("FechaAplicacion", Fecha.FechaFinal(_fecha.Month), BinaryOperatorType.LessOrEqual);
                operands[3] = new BinaryOperator("Status", MovimientoStatus.Cancelado, BinaryOperatorType.NotEqual);
                operands[4] = new BinaryOperator("Status", MovimientoStatus.Transito, BinaryOperatorType.NotEqual);

                /* operands[2] = new OperandProperty("FechaAplicacion") < new DateTime(auxfin.Year, auxfin.Month, auxfin.Day, 22, 22, 22);*/

                xpViewMovs.Criteria = new GroupOperator(operands);
                ((System.ComponentModel.ISupportInitialize)(xpViewMovs)).EndInit();
                xpViewMovs.Session = XpoDefault.Session;

                foreach (ViewRecord record in xpViewMovs)
                {
                    i = Convert.ToDateTime(record["FechaAplicacion"]).Day - 1;
                    monto = Convert.ToDecimal(record["Monto"]);
                    for (; i < days; i++)
                    {

                        saldo[i] += ((EConceptoTipo)record["ConceptoTipo"] /*as EConceptoTipo*/)
                            == EConceptoTipo.Abono ? monto : -monto;
                    }
                }
                xpViewMovs.Dispose();

                for (i = 0; i < days; i++)
                    sp += saldo[i];

                return sp / days;
            }
            catch (Exception)
            {
                return 0.0m;
            }
        }

        //#region - update Marca
        static private void updateMarca(bool opc, MovimientoB movimiento)
        {
            bool alta = true;

            if (opc == alta)
            {
                if (EsAlta(movimiento))
                {
                    // Que tan lento hace al sistema este modelado ??
                    //
                    if (/*movimiento.*/EsCargo(movimiento))
                        Marca(Math.Abs(movimiento.Monto) * -1, movimiento);
                    else
                        Marca(movimiento.Monto, movimiento);
                }
            }
            else
            {
                // Creo que una de las cosas que se pueden cambiar de un movimiento
                // es su fecha de aplicación
                //
                // Bien, esto se tiene que hacer sólo si la fecha de aplicación
                // se modificó.
                DateTime auxFecApl;
                // string auxlTipo;

                auxFecApl = movimiento.FechaAplicacion;
                // auxlTipo = movimiento.Tipo;
                //
                movimiento.FechaAplicacion = movimiento.FAplicacionBak;
                // movimiento.Tipo = (movimiento.Tipo == "C") ? "A" : "C";
                //if (movimiento.EsCargo())
                if (movimiento.ConceptoTipo != EConceptoTipo.Cargo /*.Salida*/)
                    Marca(Math.Abs(movimiento.Monto) * -1, movimiento);
                else
                    Marca(movimiento.Monto, movimiento);
                //
                movimiento.FechaAplicacion = auxFecApl;
                // movimiento.Tipo = auxlTipo;

                if (/*movimiento.*/EsCargo(movimiento))
                    Marca(Math.Abs(movimiento.Monto) * -1, movimiento);
                else
                    Marca(movimiento.Monto, movimiento);
            }
        }
        //#endregion

        static public bool EsAlta(MovimientoB mov)
        {
            return mov.Status != MovimientoStatus.Cancelado;
        }

        //#region + Saldo inicial
        // De una cuenta y con base en la clase de saldo obtener el saldo
        // inicial, de una cuenta, dada la cuenta y la fecha o el periodo que
        // se quiere. Aunque tal vez esto no tiene ahora necesariamente que
        // estar aqui, puede obtenerse directamente de la clase saldo.
        // ------------------------------------------------------------------
        // Dada una cuenta y una fecha
        // Encontramos el saldo inicial de la cuenta
        // Lo toma de la tabla de saldos.
        //
        static public decimal SldIni(DateTime _fecha, Bancaria _cuenta)
        {
            try
            {
                XPClassInfo movsClass;
                ICollection objetos;
                CriteriaOperator[] operands = new CriteriaOperator[4];
                decimal depositos;
                decimal retiros;

                operands[0] = new BinaryOperator("Cuenta", _cuenta, BinaryOperatorType.Equal);
                operands[1] = new BinaryOperator("FechaAplicacion", _fecha, BinaryOperatorType.LessOrEqual);
                operands[2] = new BinaryOperator("Status", MovimientoStatus.Cancelado, BinaryOperatorType.NotEqual);
                operands[3] = new BinaryOperator("Status", MovimientoStatus.Transito, BinaryOperatorType.NotEqual);

                depositos = 0.0m;
                retiros = 0.0m;

                // Obtain the persistent object class info required by the GetObjects method
                movsClass = XpoDefault.Session.GetClassInfo(typeof(MovimientoB));
                objetos = XpoDefault.Session.GetObjects(movsClass, GroupOperator.Combine(GroupOperatorType.And, operands), null, int.MaxValue, false, true);
                foreach (MovimientoB mov in objetos)
                {

                    if (mov.FechaAplicacion.Date < _fecha.Date)
                    {
                        if (_cuenta.Tipo != ECuentaTipo.Credito)
                        {
                            if (/*mov.*/EsCargo(mov))
                                retiros += (mov.Monto * (mov.TipoCambio > 0 ? mov.TipoCambio : 1));
                            else
                                depositos += (mov.Monto * (mov.TipoCambio > 0 ? mov.TipoCambio : 1));
                        }
                        else
                        {
                            if (/*mov.*/EsCargo(mov))
                                depositos += (mov.Monto * (mov.TipoCambio > 0 ? mov.TipoCambio : 1));
                            else
                                retiros += (mov.Monto * (mov.TipoCambio > 0 ? mov.TipoCambio : 1));
                        }
                    }
                }
                return depositos - retiros;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        //#endregion

        //#region - Marca: crea nueva marca, o actualiza
        // ------------------------------------------------------------------
        // Actualizaciones
        // Si el movimiento tiene referencia, entonces hay que buscar
        // ese movimiento y actualizarlo.
        // Vamos hacer uso de marcas por mes, para ver si con esto solucionamos
        // el problema de la movilidad de los saldos.
        //
        static private bool Marca(decimal _monto, MovimientoB movimiento)
        {
            int mes;
            int year;
            bool ok = true;
            Saldo saldo;
            string auxfec;
            CriteriaOperator left, rigth;

            // Buscamos la marca
            //
            mes = movimiento.FechaAplicacion.Month;
            year = movimiento.FechaAplicacion.Year;
            if (EsCredito(movimiento.Cuenta))
            {
                if (movimiento.Cuenta.DiaCorte > 0 && movimiento.Cuenta.DiaCorte < 32)
                {
                    if (movimiento.FechaAplicacion.Day > movimiento.Cuenta.DiaCorte)
                    {
                        mes++;
                        if (mes > 12)
                        {
                            mes = 1;
                            year++;
                        }
                    }
                }
            }

            auxfec = string.Format("{0}/{1}/{2}", movimiento.FechaAplicacion.Day.ToString().PadLeft(2, '0'), /* + "/" +*/
                mes.ToString().PadLeft(2, '0'), /* + "/" +*/
                year);
            auxfec = auxfec.Substring(6, 4) + auxfec.Substring(3, 2);

            left = new BinaryOperator("Cuenta", movimiento.Cuenta, BinaryOperatorType.Equal);
            rigth = new BinaryOperator("Periodo", auxfec, BinaryOperatorType.Equal);
            saldo = movimiento.Session.FindObject(typeof(Saldo), GroupOperator.And(left, rigth)) as Saldo;

            if (saldo == null)
            {
                // Qué pasa si hay un movimiento con fecha posterior a la que
                // vamos a dar de alta ?
                saldo = new Saldo(movimiento.Session);
                saldo.Cuenta = movimiento.Cuenta;
                saldo.Periodo = auxfec;
                saldo.Monto = SldFin(movimiento);

                // Ene 2013 Commit
                // saldo.Save();
            }
            // Por si hay marcas adelante de esta
            // Hay que recorrer todas las marcas y actualizarlas
            //
            ok = actualizaMarcas(_monto, movimiento, saldo);
            return ok;
        }
        //#endregion

        //#region - Saldo final
        // Dada una cuenta y un periodo, queremos saber cuál es el saldo
        // final, todavía no sé para qué lo necesitamos. Pero están involucradas
        // 3 clases, cuenta, saldo y movimientos.
        // ------------------------------------------------------------------
        // Retorna 0 si la cuenta no existe
        //         saldo inicial de la cuenta si no tiene movimientos
        //         saldo calculado como anterior mas abonos menos cargos.
        //         Será mejor pasar el recorrido a la clase de movs cuentas
        // Jun 2005, desafortunadamente lo que estamos haciendo, lo está haciendo
        //      incompatible con la versión actual.
        // Será cuestión de ver si podemos hacer un traductor.
        //
        // Hay que revisar mas pero hasta ahorita nov 2007 se usa para el dar de alta movimiento
        // entonces no es necesario buscar la cuenta, pues la traemos.
        //
        static private decimal SldFin(MovimientoB movimiento)
        {
            decimal fin;

            try
            {
                DateTime fecha = movimiento.FechaAplicacion.AddMonths(-1);

                fin = SldIni(fecha, movimiento.Cuenta);


                // A lo mejor puedo usar la nueva función de MontosPeriodo
                DateTime inf;
                DateTime cota;
                ConceptoB concepto;
                XPCollection objetos;
                CriteriaOperator[] operands = new CriteriaOperator[4];

                //
                inf = new DateTime(movimiento.FechaAplicacion.Year, movimiento.FechaAplicacion.Month, 1);
                inf = inf.AddMonths(-1);

                cota = new DateTime(movimiento.FechaAplicacion.Year, movimiento.FechaAplicacion.Month, 1);
                cota = cota.AddDays(-1);

                CriteriaOperator criteria = CriteriaOperator.Parse(string.Format("Clave = {0}", ClvCptIni) /*'A.SLDI'"*/);
                concepto = movimiento.Session.FindObject(typeof(ConceptoB), criteria) as ConceptoB;

                operands[0] = new BinaryOperator("Cuenta", movimiento.Cuenta, BinaryOperatorType.Equal);
                operands[1] = new BinaryOperator("FechaAplicacion", inf, BinaryOperatorType.GreaterOrEqual);
                operands[2] = new BinaryOperator("FechaAplicacion", cota, BinaryOperatorType.LessOrEqual);
                operands[3] = new BinaryOperator("Concepto", concepto, BinaryOperatorType.NotEqual);


                objetos = new XPCollection(movimiento.Session, typeof(MovimientoB), GroupOperator.Combine(GroupOperatorType.And, operands));
                foreach (MovimientoB mov in objetos)
                {
                    fin += /*mov.*/EsCargo(mov) ? -1 * mov.Monto : mov.Monto;
                }

                if (movimiento.IsNewObject() && movimiento.Concepto.Clave == ClvCptIni /*"A.SLDI"*/)
                    fin += movimiento.Monto;
            }
            catch (Exception)
            {
                if (movimiento.Concepto.Clave == ClvCptIni /*"A.SLDI"*/)
                    fin = movimiento.Cuenta.SaldoFinal;
                else
                    fin = 0.0m;
            }
            return fin;
        }
        //#endregion

        //#region + Graba agenda
        static public void GrabaAgenda(MovimientoB _movimiento)
        {
            try
            {
                if (_movimiento.Periodo != 0 && _movimiento.Periodo != EPeriodicidad.Invalido /*Abr 2013 null*/)
                {
                    Agenda agenda = new Agenda(_movimiento.Session);

                    agenda.Cuenta = _movimiento.Cuenta;
                    agenda.Concepto = _movimiento.Concepto;
                    // * Abr 2013 Me falta poner esto
                    agenda.Fecha = Periodicidad.CalculaFechas(_movimiento.Periodo, _movimiento.FechaAplicacion);
                    agenda.Monto = _movimiento.Monto;
                    agenda.Incidencias = _movimiento.Incidencias;
                    agenda.Periodo = _movimiento.Periodo;

                    // Ene 2013 Commit
                    // agenda.Save();
                }
            }
            catch (Exception)
            {
                LogDebug.Escribe("No se pudo grabar el movimiento agendado.");
                throw;
            }
        }
        //#endregion

        static public bool EsMaestra(Bancaria cnt)
        {
            return cnt.Tipo == ECuentaTipo.Maestra;
        }

        //#region + La cuenta tiene status baja?
        static public bool EsBaja(Bancaria cnt)
        {
            return cnt.Status == StatusTipoA.Baja;
        }
        //#endregion

        //#region - actualiza Marcas: actualiza saldos
        // ------------------------------------------------------------------
        // Hay que recorrer todas las marcas y actualizarlas
        // Esta rutina se cambiara a otra que recorra todos los saldos y los actualice.
        //
        static private bool actualizaMarcas(decimal _monto, MovimientoB movimiento, Saldo _saldo)
        {
            // string auxfec;
            // DateTime sig;
            XPCollection objetos;
            CriteriaOperator left, rigth;


            // sig = movimiento.FechaAplicacion;
            // sig = sig.AddMonths(1);

            /*
            auxfec = sig.Day.ToString().PadLeft(2, '0') + "/" +
                sig.Month.ToString().PadLeft(2, '0') + "/" +
                sig.Year.ToString();
            auxfec = auxfec.Substring(6, 4) + auxfec.Substring(3, 2);*/

            left = new BinaryOperator("Cuenta", movimiento.Cuenta, BinaryOperatorType.Equal);
            rigth = new BinaryOperator("Periodo", _saldo.Periodo/*auxfec*/, BinaryOperatorType.GreaterOrEqual);

            objetos = new XPCollection(movimiento.Session, typeof(Saldo), GroupOperator.And(left, rigth));

            foreach (Saldo saldo in objetos)
            {
                saldo.Monto += _monto;
                // Ene 2013 Commit
                // saldo.Save();
            }
            return true;
        }
        //#endregion













        static public bool GrabaConcepto(ConceptoB concepto)
        {
            if (!concepto.IsNewObject() && (concepto.Tipo != concepto.TipoA) && CeptoConMovs(concepto))
                throw new Exception("Imposible cambiar el Tipo. Ya tiene movimientos");

            return true;
        }

        //#region - Concepto con movimientos ?
        static private bool CeptoConMovs(ConceptoB _concepto)
        {
            CriteriaOperator criteria;
            MovimientoB mov;

            criteria = new BinaryOperator("Concepto", _concepto, BinaryOperatorType.Equal);

            mov = _concepto.Session.FindObject(typeof(MovimientoB), criteria) as MovimientoB;
            return mov != null;
        }
        //#endregion

        static public bool EsSaldoIni(ConceptoB cpt)
        {
            return cpt.Clave == "A.SLDI";
        }

        static public bool CancMov(MovimientoB movimiento)
        {
            bool ok = false;

            if (movimiento != null)
            {
                if (movimiento.Tipo == EMovimientoTipo.Gasto ||
                    !EsBaja(movimiento.Cuenta) && !EstaCancelado(movimiento))
                    ok = CancelarMovimiento(movimiento);
            }

            return ok;
        }

        static public void AppTransito(MovimientoB obj)
        {
            if (obj.Status == MovimientoStatus.Transito && obj.FechaAplicacion <= DateTime.Today)
                obj.Notas = string.Format("{0}. {1}Fecha de aplicación {2}", 
                    string.IsNullOrEmpty(obj.Notas) ? string.Empty : obj.Notas, Environment.NewLine, DateTime.Today.ToString("DD/MMM/yyyy"));
        }

        //#region + Cancelar movimiento, cheque
        static public bool CancelarMovimiento(MovimientoB _movimiento)
        {
            if (_movimiento == null)
                return false;

            bool ok = false;

            if (puedeUndoMov(_movimiento))
                if (undoMov(_movimiento))
                {
                    Cancela(_movimiento);
                    ok = true;
                }


            return ok;
        }
        //#endregion

        //#region - Puede cancelar movimiento
        // ------------------------------------------------------------------
        // May 2003, Verifica si se puede cancelar o borrar el movimiento.
        // Ojo,      Hay que hacer un proceso para eliminar los datos que ya
        //           no se quieren.
        //
        static private bool puedeUndoMov(MovimientoB _movimiento)
        {
            return _movimiento.Tipo == EMovimientoTipo.Gasto || !EsBaja(_movimiento.Cuenta);
        }
        //#endregion


        //#region - Cancela movimiento
        // ------------------------------------------------------------------
        // May 2003, Si la cuenta está dada de baja no se pueden borrar movimientos
        //           Para borrar y cancelar un movimiento
        //           Si se borra un movimiento cancelado, sólo se debe borrar.
        // Retorna 1, si se pudo para que se borre o se cancele
        //
        static private bool undoMov(MovimientoB _movimiento)
        {
            if (!EstaCancelado(_movimiento) && _movimiento.Tipo != EMovimientoTipo.Gasto)
            {
                decimal mnto;

                if (EsCargo(_movimiento))
                    mnto = Math.Abs(_movimiento.MontoAplicar);
                else
                    // Ene 17, hay abonos que pueden ser negativos, como la Afore con rendimientos negativos
                    mnto = /*Math.Abs(*/_movimiento.MontoAplicar /*)*/ * -1;

                // Jun 2002 al actualizar la marca, como se está
                // borrando el registro hay que deshacer lo que se
                // hizo.
                // Dic 09, yo digo que el monto debe quedar sin signo.
                // Ene 17, pero puedo tener un abono negativo, y debe quedar con el signo.
                // Es más no debe modificarse el monto. !
                //
                //_movimiento.Monto = Math.Abs(_movimiento.MontoAplicar); // mnto;

                // Actualizamos los restantes saldos;
                Marca(_movimiento.Monto, _movimiento);

                if (/*_movimiento.*/EsTransito(_movimiento))
                    _movimiento.Cuenta.SaldoTransito -= mnto;
                else
                {
                    if (/*_movimiento.Cuenta.*/EsCredito(_movimiento.Cuenta))
                        _movimiento.Cuenta.SaldoFinal -= mnto;
                    else
                        _movimiento.Cuenta.SaldoFinal += mnto;
                }
                // Feb 2013 se debe usar un commit
                // _movimiento.Cuenta.Save();
            }
            return true;
        }
        //#endregion

        //#region + Status cancelado
        static public void Cancela(MovimientoB mov)
        {
            mov.Status = MovimientoStatus.Cancelado;
            mov.FchCan = DateTime.Now;
        }
        //#endregion
    }
}
