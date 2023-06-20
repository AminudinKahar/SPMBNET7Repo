using SPMBNET7.CoreBusiness._Enums;
using SPMBNET7.CoreBusiness.Models.Modules._01_Jadual;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace SPMBNET7.CoreBusiness.Models.Modules._02_Akaun
{
    public class AkCimbEFT1
    {
        // note:
        // FlJenisBaucer = 0 ( Am )
        // FlJenisBaucer = 1 ( Inbois )
        // FlJenisBaucer = 2 ( Gaji )
        // FlJenisBaucer = 3 ( Pendahuluan )
        // FlJenisBaucer = 4 ( Rekupan )
        // FlJenisBaucer = 5 ( Tambah Had Panjar )
        // FlJenisBaucer = 6 ( Profil Atlet / Jurulatih )
        // ..
        // FlPenerimaEFT = 0 ( Am / Lain - lain )
        // FlPenerimaEFT = 1 ( pembekal )
        // FlPenerimaEFT = 2 ( pekerja )
        // FlPenerimaEFT = 3 ( pemegang panjar )
        // FlPenerimaEFT = 4 ( jurulatih )
        // FlPenerimaEFT = 5 ( atlet )
        public int Id { get; set; }
        public int Indek { get; set; }
        public int AkCimbEFTId { get; set; }
        public AkCimbEFT? AkCimbEFT { get; set; }
        public int AkPVId { get; set; }
        public AkPV? AkPV { get; set; }
        public KategoriPenerima FlPenerimaEFT { get; set; }
        public int? AkPembekalId { get; set; }
        public AkPembekal? AkPembekal { get; set; }
        public int? SuPekerjaId { get; set; }
        public SuPekerja? SuPekerja { get; set; }
        public int? SuJurulatihId { get; set; }
        public SuJurulatih? SuJurulatih { get; set; }
        public int? SuAtletId { get; set; }
        public SuAtlet? SuAtlet { get; set; }
        [DisplayName("Jumlah RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
        public string NoCek { get; set; } = string.Empty; // noAkaunBank
        public string Catatan { get; set; } = string.Empty;

        // note:
        // JBank - Jenis Bank Penerima
        public int? JBankId { get; set; }
        public JBank? JBank { get; set; }

        // FlStatus = 0 -> Tolak / Gagal
        // FlStatus = 1 -> Berjaya
        // FlStatus = null -> KIV
        public int? FlStatus { get; set; }
    }
}