using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using Escul.Module.BusinessObjects;
using DevExpress.Spreadsheet;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System.IO;
using System.Text;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using DevExpress.Data.Extensions;
using Escul.Module.Utilerias;

namespace Escul.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VCMateriaGrupo : ViewController
    {
        public VCMateriaGrupo()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(MateriaGrp);

            simpleActionExprtrLst.TargetViewType = ViewType.DetailView;

            simpleActionClclHrs.TargetObjectType = typeof(MateriaGrp);
            simpleActionClclHrs.TargetViewType = ViewType.DetailView;

            simpleActionClclHrsSmn.TargetObjectType = typeof(MateriaGrp);
            simpleActionClclHrsSmn.TargetViewType = ViewType.DetailView;

            simpleActionClclFchAplccn.TargetObjectType = typeof(MateriaGrp);
            simpleActionClclFchAplccn.TargetViewType = ViewType.DetailView;
            simpleActionClclFchAplccn.TargetObjectsCriteria = "HrsClndr > 0";

            simpleActionImprtLstAlmns.TargetObjectType = typeof(MateriaGrp);
            simpleActionImprtLstAlmns.TargetViewType = ViewType.DetailView;

            simpleActionClfccnFnl.TargetObjectType = typeof(MateriaGrp);
            simpleActionClfccnFnl.TargetViewType = ViewType.DetailView;

            popupWindowShowActionFlter.TargetObjectType = typeof(MateriaGrp);
            popupWindowShowActionFlter.TargetViewType = ViewType.DetailView;

            simpleActionObtnTms.TargetObjectType = typeof(MateriaGrp);
            simpleActionObtnTms.TargetViewType = ViewType.DetailView;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.

            this.simpleActionExprtrLst.Active.SetItemValue("Visible", false);
            this.popupWindowShowActionFlter.Active.SetItemValue("Visible", false);

            if (View is DetailView)
            {
                MateriaGrp mtgrp = View.CurrentObject as MateriaGrp;
                if (View.ObjectSpace.IsNewObject(mtgrp))
                    mtgrp.Changed += Mtgrp_Changed;
            }
        }

        private void Mtgrp_Changed(object sender, ObjectChangeEventArgs e)
        {
            if (e.PropertyName == "Mtr")
            {
                MateriaGrp mtgrp = View.CurrentObject as MateriaGrp;
                if (mtgrp.Temas.Count == 0)
                    Negocio.ObtenTemas(mtgrp, View.ObjectSpace);
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void simpleActionExprtrLst_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            /*
            if (View != null)
            {
                MateriaGrp obj = View.CurrentObject as MateriaGrp;

                if (obj != null)
                {
                    Workbook book = new Workbook();

                    book.LoadDocument("FO-SEP-10.xls");
                    var sheet = book.Worksheets.ActiveWorksheet;

                    short i = 19;

                    sheet.Cells["G11"].Value = obj.Mtr.Nmbr;
                    sheet.Cells["AI11"].Value = obj.Grp.Nmbr;
                    sheet.Cells["BA11"].Value = obj.Grp.Crrr.Nmbr;
                    sheet.Cells["H13"].Value = obj.Prfsr.Nombre;

                    obj.Alumnos.Sorting.Add(new SortProperty("Almn.NombreCompleto", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    foreach (AlumnoMtr alm in obj.Alumnos)
                    {
                        // sheet.Cells[i, 1].Value = i - 19;
                        sheet.Cells[i, 5].Value = alm.Almn.NombreCompleto;

                        i++;
                    }

                    string aux = string.Format("FO-SEP-10.xlsx");
                    book.SaveDocument(aux);
                }
            }*/
        }

        private void simpleActionClclHrs_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {
                MateriaGrp obj = View.CurrentObject as MateriaGrp;
                Negocio.HorasCalendario(obj);

                /*
                obj.HrsClndr = 0;
                if (obj != null && obj.Clndr != null && obj.Horarios.Count > 0)
                {
                    DateTime dia = obj.Clndr.FchIncl;
                    Horario hrr = obj.Horarios[0];
                    bool excp;

                    while (dia <= obj.Clndr.FchFnl)
                    {
                        excp = false;
                        foreach (CalendarioEvento evnt in obj.Clndr.Eventos)
                            if (!excp)
                                excp = dia == evnt.Fch;

                        if (!excp)
                        {
                            if (dia.DayOfWeek == DayOfWeek.Monday)
                            {
                                // Por el momento supondré que sólo hay un horario
                                if (hrr.TtlLns > 0)
                                    obj.HrsClndr += hrr.TtlLns;
                            }
                            else if (dia.DayOfWeek == DayOfWeek.Tuesday)
                            {
                                if (hrr.TtlMrts > 0)
                                    obj.HrsClndr += hrr.TtlMrts;
                            }
                            else if (dia.DayOfWeek == DayOfWeek.Wednesday)
                            {
                                if (hrr.TtlMrcls > 0)
                                    obj.HrsClndr += hrr.TtlMrcls;
                            }
                            else if (dia.DayOfWeek == DayOfWeek.Thursday)
                            {
                                if (hrr.TtlJvs > 0)
                                    obj.HrsClndr += hrr.TtlJvs;
                            }
                            else if (dia.DayOfWeek == DayOfWeek.Friday)
                            {
                                if (hrr.TtlVrns > 0)
                                    obj.HrsClndr += hrr.TtlVrns;
                            }
                            else if (dia.DayOfWeek == DayOfWeek.Saturday)
                            {
                                if (hrr.TtlSbd > 0)
                                    obj.HrsClndr += hrr.TtlSbd;
                            }
                        }
                        dia = dia.AddDays(1);
                    }
                }*/
            }
        }

        private void simpleActionClclHrsSmn_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {
                MateriaGrp obj = View.CurrentObject as MateriaGrp;
                Negocio.HorasSemana(obj);

                /*
                if (obj != null)
                {
                    obj.Hrs = 0;
                    foreach (Horario hr in obj.Horarios)
                    {
                        hr.TtlLns = HorasPorDia(hr.Lns);
                        hr.TtlMrts = HorasPorDia(hr.Mrts);
                        hr.TtlMrcls = HorasPorDia(hr.Mrcls);
                        hr.TtlJvs = HorasPorDia(hr.Jvs);
                        hr.TtlVrns = HorasPorDia(hr.Vrns);
                        hr.TtlSbd = HorasPorDia(hr.Sbd);

                        obj.Hrs += Convert.ToInt16(hr.TtlLns + hr.TtlMrts + hr.TtlMrcls + hr.TtlJvs + hr.TtlVrns + hr.TtlSbd);
                    }
                }*/
            }
        }

        /*
        private float HorasPorDia(string dia)
        {
            float ttl = 0;
            if (!string.IsNullOrEmpty(dia))
            {
                string[] arrtoks = dia.Split(';');

                foreach (string hrs in arrtoks)
                {
                    ttl += HorasDia(hrs);
                }
            }

            return ttl;
        }*/

        /*
        private float HorasDia(string dia)
        {
            float hrs = 0;
            if (!string.IsNullOrEmpty(dia))
            {
                string[] arrtoks = dia.Split('-');

                if (arrtoks.Length > 1)
                {
                    float ini = 0;
                    float fin = 0;

                    // Buscamos :
                    string[] min = arrtoks[0].Split(':');
                    if (min.Length > 1)
                    {
                        ini = Convert.ToSingle(min[0]);
                        ini += Convert.ToSingle(min[1])/100;
                    }
                    else
                    {
                        ini = Convert.ToSingle(arrtoks[0]);
                    }

                    min = arrtoks[1].Split(':');
                    if (min.Length > 1)
                    {
                        fin = Convert.ToSingle(min[0]);
                        fin += Convert.ToSingle(min[1])/100;
                    }
                    else
                    {
                        fin = Convert.ToSingle(arrtoks[1]);
                    }

                    if (fin >= ini)
                    {
                        hrs = Convert.ToSingle(Math.Round(fin - ini, 2));
                    }
                }
            }
            return hrs;
        }*/

        private void simpleActionClclFchAplccn_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {
                MateriaGrp obj = View.CurrentObject as MateriaGrp;
                Negocio.FechasAplicacionTemas(obj);

                /*
                if (obj != null && obj.Temas != null && obj.Temas.Count > 0 
                    && obj.Clndr != null && obj.Horarios != null && obj.Horarios.Count > 0)
                {
                    SortingCollection sortProps = new SortingCollection();
                    float hrsDspnbls = 0;
                    float hrsNecesarias;
                    float hrsCls = 0;
                    DateTime diaIni = obj.Clndr.FchIncl;


                    sortProps.Add(new SortProperty("Nmr", SortingDirection.Ascending));
                    obj.Temas.Sorting = sortProps;


                    hrsDspnbls = HrsDisponibles(ref diaIni, 
                        obj.Horarios[0], obj.Clndr.FchFnl, obj.Clndr);
                    hrsCls += hrsDspnbls;

                    if (hrsDspnbls > 0)
                    {
                        foreach (TemaMtr tm in obj.Temas)
                        {
                            hrsNecesarias = tm.Drcn;
                            tm.FchPrgrmd = diaIni;

                            if (tm.Children == null 
                                || tm.Children.Count == 0)
                            {
                                // tm.HrsClsAcmlds = hrsCls; Viene de Planeacion
                                // no sé para qué se usa Ago 2020
                                //
                                // Este día es suficiente
                                if (hrsNecesarias == hrsDspnbls)
                                {
                                    diaIni = diaIni.AddDays(1);
                                    hrsDspnbls = HrsDisponibles(ref diaIni, 
                                        obj.Horarios[0], obj.Clndr.FchFnl, 
                                        obj.Clndr);
                                    hrsCls += hrsDspnbls;
                                }
                                // Necesito otro día
                                else if (hrsNecesarias > hrsDspnbls)
                                {
                                    while (hrsNecesarias > hrsDspnbls && hrsDspnbls > 0)
                                    {
                                        hrsNecesarias -= hrsDspnbls;

                                        diaIni = diaIni.AddDays(1);
                                        hrsDspnbls = HrsDisponibles(ref diaIni, 
                                            obj.Horarios[0], obj.Clndr.FchFnl, obj.Clndr);
                                        
                                        hrsCls += hrsDspnbls;
                                    }
                                    hrsDspnbls -= hrsNecesarias;
                                }
                                // Este día sobran horas
                                else
                                {
                                    hrsDspnbls -= hrsNecesarias;
                                }

                                / *
                                if (hrsDspnbls <= 0)
                                    break;* /
                                if (hrsDspnbls <= 0)
                                {
                                    diaIni = diaIni.AddDays(1);
                                    hrsDspnbls = Negocio.HrsDisponibles(ref diaIni,
                                        obj.Horarios[0], obj.Clndr.FchFnl, obj.Clndr);

                                    hrsCls += hrsDspnbls;
                                    if (diaIni > obj.Clndr.FchFnl)
                                        break;
                                }

                            }
                        }
                    }
                }*/
            }
        }

        /*
        public float HrsDisponibles(ref DateTime diaIni, Horario hrr, 
            DateTime fin, Calendario Clndr)
        {
            float hrsDspnbls = 0;
            diaIni = diaIni.AddDays(-1);

            while (hrsDspnbls == 0 && diaIni <= fin)
            {
                diaIni = diaIni.AddDays(1);


                bool excp = false;
                foreach (CalendarioEvento evnt in Clndr.Eventos)
                    if (!excp)
                        excp = diaIni == evnt.Fch;

                if (!excp)
                {
                    if (diaIni.DayOfWeek == DayOfWeek.Monday)
                        hrsDspnbls = hrr.TtlLns;
                    else if (diaIni.DayOfWeek == DayOfWeek.Tuesday)
                        hrsDspnbls = hrr.TtlMrts;
                    else if (diaIni.DayOfWeek == DayOfWeek.Wednesday)
                        hrsDspnbls = hrr.TtlMrcls;
                    else if (diaIni.DayOfWeek == DayOfWeek.Thursday)
                        hrsDspnbls = hrr.TtlJvs;
                    else if (diaIni.DayOfWeek == DayOfWeek.Friday)
                        hrsDspnbls = hrr.TtlVrns;
                    else if (diaIni.DayOfWeek == DayOfWeek.Saturday)
                        hrsDspnbls = hrr.TtlSbd;
                }
            }
            return hrsDspnbls;
        }*/

        /// <summary>
        /// Separamos nombre de matricula con un | y la separación de los nombres será por un espacio en blanco.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleActionImprtLstAlmns_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {
                MateriaGrp obj = View.CurrentObject as MateriaGrp;

                if (obj != null)
                {
                    using (StreamReader sr = new StreamReader("ALUMNOS.txt", Encoding.Default))
                    {
                        IObjectSpace os = View.ObjectSpace;
                        GroupOperator gp = new GroupOperator();
                        Alumno almn;

                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] arrtoks = line.Split('|');

                            // Trae matrícula
                            if (arrtoks.Length > 1)
                            {
                                AlumnoMtr almmtr = os.CreateObject<AlumnoMtr>();

                                gp.Operands.Add(new BinaryOperator("Clave", arrtoks[1].Trim()));

                                almn = os.FindObject<Alumno>(gp);
                                if (almn != null)
                                {
                                    almmtr.Almn = almn;
                                }
                                else
                                {
                                    almn = os.CreateObject<Alumno>();

                                    almn.Clave = arrtoks[1].Trim();

                                    string[] subarrtoks = arrtoks[0].Split(' ');
                                    if (subarrtoks.Length > 0)
                                    {
                                        almn.Paterno = subarrtoks[0];
                                        almn.Materno = subarrtoks[1];

                                        if (subarrtoks.Length > 3)
                                            almn.Nombre = string.Format("{0} {1}", subarrtoks[2].Trim(), subarrtoks[3].Trim());
                                        else
                                            almn.Nombre = subarrtoks[2].Trim();
                                    }
                                    almmtr.Almn = almn;
                                }
                                gp.Operands.Clear();
                                obj.Alumnos.Add(almmtr);
                            }
                            // no trae matricula
                            else
                            {
                                /*
                                    gp.Operands.Add(new BinaryOperator("Paterno", arrtoks[0].Trim()));
                                    gp.Operands.Add(new BinaryOperator("Materno", arrtoks[1].Trim()));
                                    if (arrtoks.Length > )
                                        gp.Operands.Add(new BinaryOperator("Nombre", arrtoks[1]));
                                    Alumno almn = os.FindObject<Alumno>(gp);*/
                            }
                        }
                        os.CommitChanges();
                    }
                }
            }
        }

        private void simpleActionClfccnFnl_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {
                MateriaGrp mg = View.CurrentObject as MateriaGrp;

                // Para esta materia
                if (mg != null)
                {
                    // Cuántos tipos de calificaciones vamos a considerar
                    // Aquellas cuyo porcentaje > 0

                    float clfccn;
                    float[] calis = new float[mg.Modos.Count];
                    int[] many = new int[mg.Modos.Count];
                    float[] porc = new float[mg.Modos.Count];

                    for (int k = 0; k < mg.Modos.Count; k++)
                    {
                        many[k] = 0;
                        porc[k] = 0;
                    }
                    foreach (CalificacionMtr csm in mg.Calificaciones)
                    {
                        int i = 0;
                        foreach (ModoCalificacion md in mg.Modos)
                        {
                            if (csm.MdClfccn.Tp.Nombre == md.Tp.Nombre)
                            {
                                porc[i] = md.Prcntj;
                                break;
                            }
                            i++;
                        }
                        many[i]++;
                    }

                    // Por cada alumno registrado en esta materia
                    foreach (AlumnoMtr am in mg.Alumnos)
                    {
                        for (int k = 0; k < mg.Modos.Count; k++)
                        {
                            calis[k] = 0;
                        }


                        // TI Por el momento a fuerza bruta
                        // Por cada calificacion registrada al grupo
                        foreach (CalificacionMtr csm in mg.Calificaciones)
                        {
                            // Busca al alumno en esa calificación
                            foreach (CalificacionMtrAlmns cma in csm.Alumnos)
                            {
                                if (am.Almn == cma.Almn)
                                {
                                    int i = 0;
                                    foreach (ModoCalificacion md in mg.Modos)
                                    {
                                        if (csm.MdClfccn.Tp.Nombre == md.Tp.Nombre)
                                            break;
                                        i++;
                                    }
                                    calis[i] += cma.Clfccn;
                                    if (am.Clfccn > 0)
                                        i++;
                                }
                            }
                        }

                        if (am.Almn.Paterno == "LEAL")
                            clfccn = 1;
                        clfccn = 0;
                        for (int k = 0; k < mg.Modos.Count; k++)
                        {
                            if (many[k] > 0)
                                clfccn += (calis[k] / many[k]) * porc[k] / 10;
                        }

                        am.Clfccn = clfccn;
                    }
                }
            }
        }

        private void popupWindowShowActionFlter_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            FilterEvlcnCntn newObj = objectSpace.CreateObject<FilterEvlcnCntn>();
            e.View = Application.CreateDetailView(objectSpace, "FilterEvlcnCntn_DetailView", true, newObj);
        }

        private void popupWindowShowActionFlter_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            FilterEvlcnCntn objf = e.PopupWindowViewCurrentObject as FilterEvlcnCntn;

            if (objf != null)
            {
                if (View != null)
                {
                    MateriaGrp obj = View.CurrentObject as MateriaGrp;

                    if (obj != null)
                    {
                        /*List<DateTime> dates;*/
                        Workbook book = new Workbook();


                        /*
                        book.LoadDocument("FO-SEP-10.xls");*/
                        book.LoadDocument("FORMATO EXAMEN DIAGNOSTICO.xls");
                        var sheet = book.Worksheets.ActiveWorksheet;

                        short i = 19;

                        /*TIT
                        sheet.Cells["G11"].Value = obj.Mtr.Nmbr;
                        sheet.Cells["AI11"].Value = obj.Grp.Nmbr;
                        sheet.Cells["BA11"].Value = obj.Grp.Crrr.Nmbr;
                        sheet.Cells["H13"].Value = obj.Prfsr.Nombre;*/
                        sheet.Cells["C1"].Value = obj.Grp.Crrr.Nmbr;
                        sheet.Cells["F1"].Value = obj.Mtr.Nmbr;
                        sheet.Cells["F3"].Value = obj.Grp.Nmbr;
                        sheet.Cells["C3"].Value = obj.CclEsclr.Nombre;
                        sheet.Cells["A38"].Value = obj.Prfsr.Nombre;

                        /*int j;*/

                        /*TIT   Encabezado
                        j = 20;
                        dates = new List<DateTime>();
                        // Días que se reportan
                        // La fecha inicial está dentro del calendario?
                        if (obj.Clndr != null && obj.Clndr.FchIncl <= objf.FchIncl && objf.FchIncl <= obj.Clndr.FchFnl)
                        {
                            DateTime dia = objf.FchIncl;
                            while (dia <= obj.Clndr.FchFnl && j < 35)
                            {
                                if (!DiaExcep(dia, obj.Clndr))
                                {
                                    if (HayClase(dia, obj.Horarios[0]))
                                    {
                                        sheet.Cells[18, j++].Value = dia.Day;
                                        dates.Add(dia);
                                    }
                                }
                                dia = dia.AddDays(1);
                            }
                        }*/

                        // Busco la calificacion 
                        CalificacionMtr clf = obj.Calificaciones[0];

                        i = 5;
                        obj.Alumnos.Sorting.Add(new SortProperty("Almn.NombreCompleto", SortingDirection.Ascending));
                        foreach (AlumnoMtr alm in obj.Alumnos)
                        {
                            sheet.Cells[i, 1].Value = alm.Almn.Clave;
                            sheet.Cells[i, 2].Value = alm.Almn.NombreCompleto;

                            foreach (CalificacionMtrAlmns ca in clf.Alumnos)
                                if (ca.Almn.NombreCompleto == alm.Almn.NombreCompleto)
                                    sheet.Cells[i, 3].Value = ca.Clfccn.ToString();
                            /*TIT
                            j = 20;
                            obj.Asistencias.Sorting.Add(new SortProperty("Fch", SortingDirection.Ascending));
                            foreach (AsistenciaMtr asis in obj.Asistencias)
                            {
                                if (dates.Contains(asis.Fch))
                                {
                                    foreach (AsistenciaMtrAlmns asisalumn in asis.Alumnos)
                                    {
                                        if (alm.Almn == asisalumn.Almn)
                                        {                                            
                                            int day = Convert.ToInt32(sheet.Cells[18, j].Value.NumericValue);
                                         
                                            while (day < asis.Fch.Day)
                                            { 
                                                j++;
                                                day = Convert.ToInt32(sheet.Cells[18, j].Value.NumericValue);
                                            }

                                            if (asisalumn.Asstnc)
                                                sheet.Cells[i, j++].Value = "A";
                                            else
                                                sheet.Cells[i, j++].Value = "F";
                                        }
                                    }
                                }

                            }*/
                            i++;
                        }

                        string aux = string.Format("FO-SEP-10.xlsx");
                        book.SaveDocument(aux);
                    }
                }
            }
        }

        /*
        private bool DiaExcep(DateTime dia, Calendario clndr)
        {
            bool excp = false;

            / *
            while (dia <= clndr.FchFnl)
            {
                excp = false;* /
            foreach (CalendarioEvento evnt in clndr.Eventos)
                if (!excp)
                    excp = dia == evnt.Fch;
            / *
        }* /

            return excp;
        }*/

        /*
        private bool HayClase(DateTime dia, Horario hrr)
        {
            if (dia.DayOfWeek == DayOfWeek.Monday)
            {
                // Por el momento supondré que sólo hay un horario
                return hrr.TtlLns > 0;
            }
            else if (dia.DayOfWeek == DayOfWeek.Tuesday)
            {
                return hrr.TtlMrts > 0;
            }
            else if (dia.DayOfWeek == DayOfWeek.Wednesday)
            {
                return hrr.TtlMrcls > 0;
            }
            else if (dia.DayOfWeek == DayOfWeek.Thursday)
            {
                return hrr.TtlJvs > 0;
            }
            else if (dia.DayOfWeek == DayOfWeek.Friday)
            {
                return hrr.TtlVrns > 0;
            }
            else if (dia.DayOfWeek == DayOfWeek.Saturday)
            {
                return hrr.TtlSbd > 0;
            }
            else
                return false;
        }*/

        private void popupWindowShowActionFOSEP09_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            FilterEvlcnCntn newObj = objectSpace.CreateObject<FilterEvlcnCntn>();
            e.View = Application.CreateDetailView(objectSpace, "FilterEvlcnCntn_DetailView", true, newObj);
        }

        private void popupWindowShowActionFOSEP09_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            FilterEvlcnCntn objf = e.PopupWindowViewCurrentObject as FilterEvlcnCntn;

            if (objf != null)
            {
                if (View != null)
                {
                    MateriaGrp obj = View.CurrentObject as MateriaGrp;

                    if (obj != null)
                    {
                        if (objf.File.FileName == "FO-SEP-09.xls")
                            FOSEP09(obj);
                        else if (objf.File.FileName == "FORMATO EXAMEN DIAGNOSTICO.xls")
                            Diagnostico(obj);
                        else if (objf.File.FileName == "ListaAsistencia.xls")
                            ListaAsistencia(obj, objf);
                        else if (objf.File.FileName == "FO-SEP-10.xls")
                            FOSEP10(obj, objf);
                        else if (objf.File.FileName == "FORMATO PLANEACION.xls")
                            Planeacion(obj, objf);
                        else if (objf.File.FileName == "CoEvaluacion.xls")
                            CoEvaluacion(obj, objf);
                    }
                }
            }

        }

        private void popupWindowShowActionClclClfccn_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            FilterCalclCalfccn newObj = objectSpace.CreateObject<FilterCalclCalfccn>();
            e.View = Application.CreateDetailView(objectSpace, "FilterCalclCalfccn_DetailView", true, newObj);
        }

        private void popupWindowShowActionClclClfccn_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            // Creo que debe quedar así y no como lo puse arriba, tal vez hay que quitar el código de arriba.

            FilterCalclCalfccn objf = e.PopupWindowViewCurrentObject as FilterCalclCalfccn;

            if (objf != null && View != null)
            {
                MateriaGrp mg = View.CurrentObject as MateriaGrp;

                // Para esta materia
                if (mg != null)
                {
                    // Cuántos tipos de calificaciones vamos a considerar
                    // Aquellas cuyo porcentaje > 0

                    float clfccn;
                    float[] calis = new float[mg.Modos.Count];
                    int[] many = new int[mg.Modos.Count];
                    float[] porc = new float[mg.Modos.Count];

                    for (int k = 0; k < mg.Modos.Count; k++)
                    {
                        many[k] = 0;
                        porc[k] = 0;
                    }

                    GroupOperator gpf = new GroupOperator();
                    gpf.Operands.Add(new BinaryOperator("Fch", apl.Log.Fecha.FechaInicial(objf.FchIncl), BinaryOperatorType.GreaterOrEqual));
                    gpf.Operands.Add(new BinaryOperator("Fch", apl.Log.Fecha.FechaInicial(objf.FchFnl), BinaryOperatorType.LessOrEqual));
                    if (objf.MdClfccn != null)
                        gpf.Operands.Add(new BinaryOperator("MdClfccn.Tp.Nombre", objf.MdClfccn.Tp.Nombre));

                    mg.Calificaciones.Filter = gpf;
                    foreach (CalificacionMtr csm in mg.Calificaciones)
                    {
                        int i = 0;
                        foreach (ModoCalificacion md in mg.Modos)
                        {
                            if (csm.MdClfccn == md)
                            {
                                porc[i] = md.Prcntj;
                                break;
                            }
                            i++;
                        }
                        many[i]++;
                    }

                    // Por cada alumno registrado en esta materia
                    foreach (AlumnoMtr am in mg.Alumnos)
                    {
                        for (int k = 0; k < mg.Modos.Count; k++)
                        {
                            calis[k] = 0;
                        }


                        // TI Por el momento a fuerza bruta
                        // Por cada calificacion registrada al grupo
                        foreach (CalificacionMtr csm in mg.Calificaciones)
                        {
                            // Busca al alumno en esa calificación
                            foreach (CalificacionMtrAlmns cma in csm.Alumnos)
                            {
                                if (am.Almn == cma.Almn)
                                {
                                    int i = 0;
                                    foreach (ModoCalificacion md in mg.Modos)
                                    {
                                        if (csm.MdClfccn == md)
                                            break;
                                        i++;
                                    }
                                    calis[i] += cma.Clfccn;
                                    if (am.Clfccn > 0)
                                        i++;
                                }
                            }
                        }

                        if (am.Almn.Paterno == "CAMACHO")
                            clfccn = 1;
                        clfccn = 0;
                        for (int k = 0; k < mg.Modos.Count; k++)
                        {
                            if (many[k] > 0)
                                clfccn += (calis[k] / many[k]) * porc[k] / 100;
                        }

                        am.TmpClfccn = clfccn;

                    }


                    CalificacionMtr cm = View.ObjectSpace.CreateObject<CalificacionMtr>();
                    cm.Fch = DateTime.Today;
                    foreach (AlumnoMtr am in mg.Alumnos)
                    {
                        CalificacionMtrAlmns almclf = View.ObjectSpace.CreateObject<CalificacionMtrAlmns>();

                        almclf.Almn = am.Almn;
                        almclf.Clfccn = am.TmpClfccn;

                        cm.Alumnos.Add(almclf);
                    }
                    mg.Calificaciones.Add(cm);
                }
            }

        }

        private void FOSEP09(MateriaGrp obj)
        {
            Workbook book = new Workbook();

            book.LoadDocument("FO-SEP-09.xls");
            var sheet = book.Worksheets.ActiveWorksheet;

            sheet.Cells["C10"].Value = obj.Grp.Crrr.Nmbr;
            sheet.Cells["C56"].Value = obj.Grp.Crrr.Nmbr;
            sheet.Cells["C102"].Value = obj.Grp.Crrr.Nmbr;
            sheet.Cells["C148"].Value = obj.Grp.Crrr.Nmbr;
            sheet.Cells["C194"].Value = obj.Grp.Crrr.Nmbr;
            sheet.Cells["O10"].Value = obj.Mtr.Nmbr;
            sheet.Cells["O56"].Value = obj.Mtr.Nmbr;
            sheet.Cells["O102"].Value = obj.Mtr.Nmbr;
            sheet.Cells["O148"].Value = obj.Mtr.Nmbr;
            sheet.Cells["O194"].Value = obj.Mtr.Nmbr;
            sheet.Cells["AA10"].Value = obj.Grp.Nmbr;
            sheet.Cells["AA56"].Value = obj.Grp.Nmbr;
            sheet.Cells["AA102"].Value = obj.Grp.Nmbr;
            sheet.Cells["AA148"].Value = obj.Grp.Nmbr;
            sheet.Cells["AA194"].Value = obj.Grp.Nmbr;

            sheet.Cells["C11"].Value = obj.Prfsr.Nombre;
            sheet.Cells["C57"].Value = obj.Prfsr.Nombre;
            sheet.Cells["C103"].Value = obj.Prfsr.Nombre;
            sheet.Cells["C149"].Value = obj.Prfsr.Nombre;
            sheet.Cells["C195"].Value = obj.Prfsr.Nombre;
            sheet.Cells["O11"].Value = obj.CclEsclr.Nombre;
            sheet.Cells["O57"].Value = obj.CclEsclr.Nombre;
            sheet.Cells["O103"].Value = obj.CclEsclr.Nombre;
            sheet.Cells["O149"].Value = obj.CclEsclr.Nombre;
            sheet.Cells["O195"].Value = obj.CclEsclr.Nombre;
            sheet.Cells["AA11"].Value = obj.Clndr.FchIncl.ToString("MMM/yyyy");
            sheet.Cells["AA57"].Value = obj.Clndr.FchIncl.ToString("MMM/yyyy");
            sheet.Cells["AA103"].Value = obj.Clndr.FchIncl.ToString("MMM/yyyy");
            sheet.Cells["AA149"].Value = obj.Clndr.FchIncl.ToString("MMM/yyyy");
            sheet.Cells["AA195"].Value = obj.Clndr.FchIncl.ToString("MMM/yyyy");

            int t = 16;
            //int d;

            obj.Mtr.Temas.Sorting.AddRange(new SortProperty[] { new SortProperty("Nmr", SortingDirection.Ascending) });

            foreach (Tema tm in obj.Mtr.Temas)
            {
                if (tm.Nmr.Contains("."))
                {
                    if (!tm.Dscrpcn.Contains("Test"))
                    {
                        sheet.Cells[string.Format("B{0}", t)].Value = string.Format("{0} {1}", tm.Nmr, tm.Dscrpcn);
                        sheet.Cells[t - 1, tm.FchAplccn.Day + 3].Value = "X";
                        t += 2;
                    }
                }

                if (t == 38)
                    t = 62;
                else if (t == 84)
                    t = 108;
                else if (t == 130)
                    t = 154;
                else if (t == 176)
                    t = 200;
                else if (t == 224)
                    t = 246;
            }


            string aux = string.Format("FO-SEP-09.xlsx");
            book.SaveDocument(aux);
        }

        private void FOSEP10(MateriaGrp obj, FilterEvlcnCntn objf)
        {
            if (objf != null)
            {
                if (View != null)
                {
                    if (obj != null)
                    {
                        List<DateTime> dates;
                        Workbook book = new Workbook();


                        book.LoadDocument("FO-SEP-10.xls");
                        var sheet = book.Worksheets.ActiveWorksheet;

                        short i = 19;

                        sheet.Cells["G11"].Value = obj.Mtr.Nmbr;
                        sheet.Cells["AI11"].Value = obj.Grp.Nmbr;
                        sheet.Cells["BA11"].Value = obj.Grp.Crrr.Nmbr;
                        sheet.Cells["H13"].Value = obj.Prfsr.Nombre;

                        int j;

                        j = 20;
                        dates = new List<DateTime>();
                        // Días que se reportan
                        // La fecha inicial está dentro del calendario?
                        if (obj.Clndr != null && obj.Clndr.FchIncl <= objf.FchIncl && objf.FchIncl <= obj.Clndr.FchFnl)
                        {
                            DateTime dia = objf.FchIncl;
                            while (dia <= obj.Clndr.FchFnl && j < 35)
                            {
                                if (!Negocio.DiaExcep(dia, obj.Clndr))
                                {
                                    if (Negocio.HayClase(dia, obj.Horarios[0]))
                                    {
                                        sheet.Cells[18, j++].Value = dia.Day;
                                        dates.Add(dia);
                                    }
                                }
                                dia = dia.AddDays(1);
                            }
                        }

                        // Busco la calificacion 
                        CalificacionMtr clf = obj.Calificaciones[0];

                        i = 5;
                        obj.Alumnos.Sorting.Add(new SortProperty("Almn.NombreCompleto", SortingDirection.Ascending));
                        foreach (AlumnoMtr alm in obj.Alumnos)
                        {
                            sheet.Cells[i, 1].Value = alm.Almn.Clave;
                            sheet.Cells[i, 2].Value = alm.Almn.NombreCompleto;

                            foreach (CalificacionMtrAlmns ca in clf.Alumnos)
                                if (ca.Almn.NombreCompleto == alm.Almn.NombreCompleto)
                                    sheet.Cells[i, 3].Value = ca.Clfccn.ToString();

                            j = 20;
                            obj.Asistencias.Sorting.Add(new SortProperty("Fch", SortingDirection.Ascending));
                            foreach (AsistenciaMtr asis in obj.Asistencias)
                            {
                                if (dates.Contains(asis.Fch))
                                {
                                    foreach (AsistenciaMtrAlmns asisalumn in asis.Alumnos)
                                    {
                                        if (alm.Almn == asisalumn.Almn)
                                        {                                            
                                            int day = Convert.ToInt32(sheet.Cells[18, j].Value.NumericValue);
                                         
                                            while (day < asis.Fch.Day)
                                            { 
                                                j++;
                                                day = Convert.ToInt32(sheet.Cells[18, j].Value.NumericValue);
                                            }

                                            if (asisalumn.Asstnc)
                                                sheet.Cells[i, j++].Value = "A";
                                            else
                                                sheet.Cells[i, j++].Value = "F";
                                        }
                                    }
                                }

                            }
                            i++;
                        }

                        string aux = string.Format("FO-SEP-10.xlsx");
                        book.SaveDocument(aux);
                    }
                }
            }
        }

        private void Diagnostico(MateriaGrp obj)
        {
            Workbook book = new Workbook();

            book.LoadDocument("FORMATO EXAMEN DIAGNOSTICO.xls");
            var sheet = book.Worksheets.ActiveWorksheet;

            sheet.Cells["C1"].Value = obj.Grp.Crrr.Nmbr;
            sheet.Cells["F1"].Value = obj.Mtr.Nmbr;
            sheet.Cells["C3"].Value = obj.CclEsclr.Nombre;
            sheet.Cells["F3"].Value = obj.Grp.Nmbr;

            int r, c;
            r = 5;
            c = 1;

            SortingCollection sorting = new SortingCollection();
            sorting.Add(new SortProperty("Almn.NombreCompleto", SortingDirection.Ascending));
            obj.Alumnos.Sorting = sorting;
            foreach (AlumnoMtr am in obj.Alumnos)
            {
                sheet.Cells[r, c].Value = am.Almn.Clave;
                sheet.Cells[r, c + 1].Value = am.Almn.NombreCompleto;

                var es = new EmployeeSearch(am.Almn.Clave);
                sheet.Cells[r, c + 2].Value = obj.Calificaciones[0].Alumnos[obj.Calificaciones[0].Alumnos.FindIndex(es.IsClave)].Clfccn;

                r++;
            }
            sheet.Cells["A38"].Value = obj.Prfsr.Nombre;
            sheet.Cells["G38"].Value = DateTime.Today.ToShortDateString();

            string aux = string.Format("FORMATO EXAMEN DIAGNOSTICO.xlsx");
            book.SaveDocument(aux);
        }

        private void ListaAsistencia(MateriaGrp obj, FilterEvlcnCntn objf)
        {
            Workbook book = new Workbook();

            book.LoadDocument("ListaAsistencia.xlsx");
            var sheet = book.Worksheets.ActiveWorksheet;

            if (obj.Grp != null)
            {
                if (obj.Grp.Crrr != null)
                    sheet.Cells["B6"].Value = obj.Grp.Crrr.Nmbr;

                sheet.Cells["B8"].Value = obj.Grp.Nmbr;
            }
            if (obj.Mtr != null)
                sheet.Cells["B7"].Value = obj.Mtr.Nmbr;
            if (obj.CclEsclr != null)
                sheet.Cells["B9"].Value = obj.CclEsclr.Nombre;
            if (obj.Prfsr != null)
                sheet.Cells["B10"].Value = obj.Prfsr.Nombre;

            int r, c;
            r = 11;
            c = 0;

            SortingCollection sorting = new SortingCollection();
            sorting.Add(new SortProperty("Almn.NombreCompleto", SortingDirection.Ascending));
            obj.Alumnos.Sorting = sorting;

            foreach (AlumnoMtr am in obj.Alumnos)
            {
                sheet.Cells[r, c].Value = r-10;
                sheet.Cells[r, c + 1].Value = am.Almn.NombreCompleto;
                sheet.Cells[r, c + 2].Value = am.Almn.Clave;

                r++;
            }


            int j = 3;
            // Supongré dos meses a lo más.
            string mss = string.Empty;
            List<DateTime> dates;
            dates = new List<DateTime>();
            // Días que se reportan
            // La fecha inicial está dentro del calendario?
            if (obj.Clndr != null && !(objf.FchFnl < obj.Clndr.FchIncl  || objf.FchIncl > obj.Clndr.FchFnl))
            {
                DateTime dia = objf.FchIncl;
                mss = string.Format("{0}", dia.ToString("MMMM"));
                while (dia <= obj.Clndr.FchFnl && dia <= objf.FchFnl)
                {
                    if (dia >= obj.Clndr.FchIncl)
                    {
                        if (!Negocio.DiaExcep(dia, obj.Clndr))
                        {
                            if (Negocio.HayClase(dia, obj.Horarios[0]))
                            {
                                sheet.Cells[10, j++].Value = dia.Day;
                                dates.Add(dia);
                            }
                        }
                    }
                    dia = dia.AddDays(1);
                }
                if (dia.Month != objf.FchIncl.Month)
                    mss = string.Format("{0} / {1}", mss, dia./*Month.*/ToString("MMMM"));

                sheet.Cells[9, 3].Value = mss;
            }

            string aux = string.Format("ListaAsistencia{0}{1}.xlsx", obj.Grp.Nmbr, DateTime.Today.ToString("dd.MMM"));
            book.SaveDocument(aux);
        }

        private void simpleActionObtnTms_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null && View.CurrentObject != null)
            {
                MateriaGrp mg = View.CurrentObject as MateriaGrp;
                Negocio.ObtenTemas(mg, View.ObjectSpace);

                /*
                SortingCollection sortProps = new SortingCollection();
                TemaMtr padre = null;


                sortProps.Add(new SortProperty("Nmr", SortingDirection.Ascending));
                mg.Mtr.Temas.Sorting = sortProps;


                foreach (Tema tm in mg.Mtr.Temas)
                {
                    TemaMtr tmM = View.ObjectSpace.CreateObject<TemaMtr>();

                    tmM.Nmr = tm.Nmr;
                    tmM.Dscrpcn = tm.Dscrpcn;
                    tmM.Drcn = tm.Drcn;
                    tmM.FchPrgrmd = tm.FchAplccn;
                    tmM.CmptncEspcfc = tm.CmptncEspcfc;
                    tmM.CmptncGnrc = tm.CmptncGnrc;
                    tmM.Actvdds = tm.Actvdds;


                    if (tm.TemaPadre != null)
                        tmM.TemaPadre = padre;
                    if (tm.Children.Count > 0)
                        padre = tmM;


                    mg.Temas.Add(tmM);

                    // View.ObjectSpace.CommitChanges();
                }
                / *
                foreach (Tema tm in mg.Mtr.Temas)
                {
                    if (tm.TemaPadre != null)
                    {
                        TemaMtr tmM = View.ObjectSpace.CreateObject<TemaMtr>();

                        tmM.Nmr = tm.Nmr;
                        tmM.Dscrpcn = tm.Dscrpcn;
                        tmM.Drcn = tm.Drcn;
                        tmM.FchPrgrmd = tm.FchAplccn;
                        tmM.CmptncEspcfc = tm.CmptncEspcfc;
                        tmM.CmptncGnrc = tm.CmptncGnrc;
                        tmM.Actvdds = tm.Actvdds;

                        tmM.TemaPadre = View.ObjectSpace.FindObject<TemaMtr>(new BinaryOperator("Nmr", tm.TemaPadre.Nmr));
                        mg.Temas.Add(tmM);
                    }
                }*/
            }
        }

        private void Planeacion(MateriaGrp obj, FilterEvlcnCntn objf)
        {
            Workbook book = new Workbook();
            string hj = objf.NmbrHj;

            book.LoadDocument("FORMATO PLANEACION.xls");
            var sheet = book.Worksheets[hj]; // "FORMATO PARCIAL NO1"];

            if (sheet != null)
            {
                sheet.Cells["B1"].Value = obj.Grp.Crrr.Nmbr;
                sheet.Cells["C2"].Value = obj.Mtr.Nmbr;
                sheet.Cells["B4"].Value = obj.Grp.Nmbr;
                sheet.Cells["B5"].Value = obj.CclEsclr.Nombre;
                sheet.Cells["C6"].Value = DateTime.Today.ToShortDateString();
                sheet.Cells["D1"].Value = obj.Prfsr.Nombre;
            }

            int r = 8;
            string tms = string.Empty;
            SortingCollection sortProps = new SortingCollection();


            sortProps.Add(new SortProperty("FchPrgrmd", SortingDirection.Ascending));
            sortProps.Add(new SortProperty("Nmr", SortingDirection.Ascending));
            obj.Temas.Sorting.Add(sortProps);
            foreach (TemaMtr tm in obj.Temas)
            {
                if (tm.FchPrgrmd >= objf.FchIncl && tm.FchPrgrmd <= objf.FchFnl)
                {
                    sheet.Cells[r, 0].Value = string.Format("{0} {1}", tm.Nmr, tm.Dscrpcn);
                    sheet.Cells[r, 1].Value = tm.FchPrgrmd.ToShortDateString();
                    sheet.Cells[r, 2].Value = tm.Actvdds;
                    sheet.Cells[r, 3].Value = tm.CmptncEspcfc;

                    if (!string.IsNullOrEmpty(tm.Nmr) && !tm.Nmr.Contains("."))
                        tms = string.Format("{0}  {1} {2}", tms, tm.Nmr, tm.Dscrpcn);
                    r++;
                }
            }
            sheet.Cells["B7"].Value = tms;

            string aux = string.Format("FORMATO PLANEACION{0}.xlsx", obj.Grp.Nmbr);
            book.SaveDocument(aux);
        }

        private void CoEvaluacion(MateriaGrp obj, FilterEvlcnCntn objf)
        {
            Workbook book = new Workbook();

            book.LoadDocument("CoEvaluacion.xlsx");
            var sheet = book.Worksheets.ActiveWorksheet;

            sheet.Cells["B6"].Value = obj.Grp.Crrr.Nmbr;
            sheet.Cells["B7"].Value = obj.Mtr.Nmbr;
            sheet.Cells["B8"].Value = obj.Grp.Nmbr;
            sheet.Cells["B9"].Value = obj.CclEsclr.Nombre;
            sheet.Cells["B10"].Value = obj.Prfsr.Nombre;

            int r, c;
            r = 11;
            c = 0;

            SortingCollection sorting = new SortingCollection();
            sorting.Add(new SortProperty("Almn.NombreCompleto", SortingDirection.Ascending));
            obj.Alumnos.Sorting = sorting;

            SortingCollection sortingcal = new SortingCollection();
            sortingcal.Add(new SortProperty("Fch", SortingDirection.Ascending));
            obj.Calificaciones.Sorting = sortingcal;

            foreach (AlumnoMtr am in obj.Alumnos)
            {
                sheet.Cells[r, c].Value = r - 10;
                sheet.Cells[r, c + 1].Value = am.Almn.NombreCompleto;
                sheet.Cells[r, c + 2].Value = am.Almn.Clave;

                foreach (CalificacionMtr cl in obj.Calificaciones)
                {
                    if (cl.Fch >= objf.FchIncl && cl.Fch <= objf.FchFnl)
                    {
                        cl.Alumnos.Filter = new BinaryOperator("Almn.Clave", am.Almn.Clave);

                        if (cl.Alumnos.Count > 0)
                        {
                            if (cl.MdClfccn.Prcntj == 50f)
                                sheet.Cells[r, 5].Value = cl.Alumnos[0].Clfccn;
                            else if (cl.MdClfccn.Prcntj == 20f)
                                sheet.Cells[r, 4].Value = cl.Alumnos[0].Clfccn;
                            else
                            if (cl.MdClfccn.Prcntj == 30f)
                                sheet.Cells[r, 3].Value = cl.Alumnos[0].Clfccn;
                        }
                    }
                }

                r++;
            }

            /*
            int j = 3;
            List<DateTime> dates;
            dates = new List<DateTime>();
            // Días que se reportan
            // La fecha inicial está dentro del calendario?
            if (obj.Clndr != null && !(objf.FchFnl < obj.Clndr.FchIncl || objf.FchIncl > obj.Clndr.FchFnl))
            {
                DateTime dia = objf.FchIncl;
                while (dia <= obj.Clndr.FchFnl && dia <= objf.FchFnl)
                {
                    if (dia >= obj.Clndr.FchIncl)
                    {
                        if (!DiaExcep(dia, obj.Clndr))
                        {
                            if (HayClase(dia, obj.Horarios[0]))
                            {
                                sheet.Cells[10, j++].Value = dia.Day;
                                dates.Add(dia);
                            }
                        }
                    }
                    dia = dia.AddDays(1);
                }
            }*/

            string aux = string.Format("CoEvaluacion{0}{1}.xlsx", obj.Grp.Nmbr, DateTime.Today.ToString("dd.MMM"));
            book.SaveDocument(aux);

        }

    }
}
