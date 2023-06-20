﻿using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun;
using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels;
using SPMBNET7.CoreBusiness.Models.Modules._91_Permohonan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.Infrastructure.Interfaces._02_Akaun
{
    public interface BelanjawanSemasaIRepository<T1, T2>
    {
        Task<List<AbWaran>> GetAbWaranBasedOnYear(T1 tahun, T2 jKWId, int jBahagianId, DateTime tarHingga);

        List<AbBelanjawanSemasaViewModel> RunWaranObjekOperation(
            int Bahagian,
            int JenisWaran,
            string TK,
            decimal Amaun,
            string KodCarta,
            string Perihal,
            string Paras);

        Task<List<SpPendahuluanPelbagai>> GetSpPendahuluanPelbagaiBasedOnYear(T1 tahun, T2 jKWId, int jBahagianId, DateTime tarHingga);

        Task<List<AkPO>> GetAkPOBasedOnYear(T1 tahun, T2 jKWId, int jBahagianId, DateTime tarHingga);

        Task<List<AkPOLaras>> GetAkPOLarasBasedOnYear(T1 tahun, T2 jKWId, int jBahagianId, DateTime tarHingga);

        Task<List<AkInden>> GetAkIndenBasedOnYear(T1 tahun, T2 jKWId, int jBahagianId, DateTime tarHingga);

        Task<List<AkTunaiCV>> GetAkTunaiCVBasedOnYear(T1 tahun, T2 jKWId, int jBahagianId, DateTime tarHingga);

        List<AbBelanjawanSemasaViewModel> RunSpPOPOLarasIndenCVObjekOperation(
            int Bahagian,
            decimal Amaun,
            string KodCarta,
            string Perihal,
            string Paras);

        Task<List<AkPV>> GetAkPVBasedOnYear(T1 tahun, T2 jKWId, int jBahagianId, DateTime tarHingga);

        List<AbBelanjawanSemasaViewModel> RunBaucerObjekOperation(
            int Bahagian,
           bool Tanggungan,
           bool Pendahuluan,
           decimal Amaun,
           string KodCarta,
           string Perihal,
           string Paras);

        Task<List<AkTerima>> GetAkTerimaBasedOnYear(T1 tahun, T2 jKWId, int jBahagianId, DateTime tarHingga);

        List<AbBelanjawanSemasaViewModel> RunResitObjekOperation(
            int Bahagian,
           decimal Amaun,
           string KodCarta,
           string Perihal,
           string Paras);

        Task<List<AkJurnal>> GetAkJurnalBasedOnYear(T1 tahun, T2 jKWId, int jBahagianId, DateTime tarHingga);

        List<AbBelanjawanSemasaViewModel> RunJurnalObjekOperation(
            int Bahagian,
           decimal Debit,
           decimal Kredit,
           string KodCarta,
           string Perihal,
           string Paras);
    }
}
