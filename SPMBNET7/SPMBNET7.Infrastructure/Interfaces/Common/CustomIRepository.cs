using SPMBNET7.CoreBusiness.Models.Modules._02_Akaun.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.Infrastructure.Interfaces.Common
{
    public interface CustomIRepository<T1, T2>
    {
        Task<decimal> GetBalanceFromAbBukuVot(T1 tahun, int? akCartaId, int jKW, int? jBahagian);
        Task<decimal> GetBalanceFromKaunterPanjar(T1 bakiAwal, T2 akTunaiRuncitId);
        // Penyata Buku Tunai
        Task<decimal> GetCarryPreviousBalanceBasedOnStartingDate(T2 akBankId, int? JKWId, int? JBahagianId, DateTime TarMula);
        Task<List<AbBukuTunaiViewModel>> GetListBukuTunaiBasedOnRangeDate(T2 akBankId, int? JKWId, int? JBahagianId, DateTime TarMula, DateTime TarHingga);
        // Penyata Buku Tunai END

        // Penyata Alir Tunai
        Task<AbAlirTunaiViewModel> GetCarryPreviousBalanceEachStartingMonth(T2 akBankId, int? JKWId, int? JBahagianId, string Tahun);
        Task<List<AbAlirTunaiViewModel>> GetCarryPreviousBalanceEachStartingMonthDebug(T2 akBankId, int? JKWId, int? JBahagianId, string Tahun);
        Task<List<AbAlirTunaiViewModel>> GetListAlirTunaiBasedOnComparedYear(int akBankId, int? JKWId, int? JBahagianId, string Tahun1, string Tahun2, string Tahun3);
        Task<List<AbAlirTunaiViewModel>> GetListAlirTunaiMasukBasedOnYear(T2 akBankId, int? JKWId, int? JBahagianId, string Tahun);
        Task<List<AbAlirTunaiViewModel>> GetListAlirTunaiKeluarBasedOnYear(T2 akBankId, int? JKWId, int? JBahagianId, string Tahun);
        // Penyata Alir Tunai END

        // Timbang Duga 
        Task<List<AbTimbangDugaViewModel>> GetListTimbangDugaBasedOnLastDate(T2 JBahagianId, int? JKWId, DateTime TarHingga);
        // Timbang Duga END

        // Untung Rugi
        Task<List<AbUntungRugiViewModel>> GetListUntungRugiBasedOnRangeDate(T2 JBahagianId, int? JKWId, DateTime TarDari, DateTime TarHingga);
        // Untung Rugi END

        // Kunci Kira-kira
        Task<List<AbKunciKiraKiraViewModel>> GetListKunciKirakiraBasedOnLastDate(T2 JBahagianId, int? JKWId, DateTime TarHingga);
        // Kunci Kira-kira END
    }
}
