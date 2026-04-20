using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Process.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UPDATE_FLAG_NAMEESP_INDICATIVE_IN_COUNTRY : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            UPDATE Country SET NameEsp = N'Afganistán', Indicative = '93', Flag = 'https://flagcdn.com/af.svg' WHERE CountryId = 1;
UPDATE Country SET NameEsp = N'Albania', Indicative = '355', Flag = 'https://flagcdn.com/al.svg' WHERE CountryId = 2;
UPDATE Country SET NameEsp = N'Argelia', Indicative = '213', Flag = 'https://flagcdn.com/dz.svg' WHERE CountryId = 3;
UPDATE Country SET NameEsp = N'Andorra', Indicative = '376', Flag = 'https://flagcdn.com/ad.svg' WHERE CountryId = 4;
UPDATE Country SET NameEsp = N'Angola', Indicative = '244', Flag = 'https://flagcdn.com/ao.svg' WHERE CountryId = 5;
UPDATE Country SET NameEsp = N'Argentina', Indicative = '54', Flag = 'https://flagcdn.com/ar.svg' WHERE CountryId = 6;
UPDATE Country SET NameEsp = N'Armenia', Indicative = '374', Flag = 'https://flagcdn.com/am.svg' WHERE CountryId = 7;
UPDATE Country SET NameEsp = N'Australia', Indicative = '61', Flag = 'https://flagcdn.com/au.svg' WHERE CountryId = 8;
UPDATE Country SET NameEsp = N'Austria', Indicative = '43', Flag = 'https://flagcdn.com/at.svg' WHERE CountryId = 9;
UPDATE Country SET NameEsp = N'Azerbaiyán', Indicative = '994', Flag = 'https://flagcdn.com/az.svg' WHERE CountryId = 10;
UPDATE Country SET NameEsp = N'Bahamas', Indicative = '1-242', Flag = 'https://flagcdn.com/bs.svg' WHERE CountryId = 11;
UPDATE Country SET NameEsp = N'Baréin', Indicative = '973', Flag = 'https://flagcdn.com/bh.svg' WHERE CountryId = 12;
UPDATE Country SET NameEsp = N'Bangladés', Indicative = '880', Flag = 'https://flagcdn.com/bd.svg' WHERE CountryId = 13;
UPDATE Country SET NameEsp = N'Barbados', Indicative = '1-246', Flag = 'https://flagcdn.com/bb.svg' WHERE CountryId = 14;
UPDATE Country SET NameEsp = N'Bielorrusia', Indicative = '375', Flag = 'https://flagcdn.com/by.svg' WHERE CountryId = 15;
UPDATE Country SET NameEsp = N'Bélgica', Indicative = '32', Flag = 'https://flagcdn.com/be.svg' WHERE CountryId = 16;
UPDATE Country SET NameEsp = N'Belice', Indicative = '501', Flag = 'https://flagcdn.com/bz.svg' WHERE CountryId = 17;
UPDATE Country SET NameEsp = N'Benín', Indicative = '229', Flag = 'https://flagcdn.com/bj.svg' WHERE CountryId = 18;
UPDATE Country SET NameEsp = N'Bután', Indicative = '975', Flag = 'https://flagcdn.com/bt.svg' WHERE CountryId = 19;
UPDATE Country SET NameEsp = N'Bolivia', Indicative = '591', Flag = 'https://flagcdn.com/bo.svg' WHERE CountryId = 20;
UPDATE Country SET NameEsp = N'Bosnia y Herzegovina', Indicative = '387', Flag = 'https://flagcdn.com/ba.svg' WHERE CountryId = 21;
UPDATE Country SET NameEsp = N'Botsuana', Indicative = '267', Flag = 'https://flagcdn.com/bw.svg' WHERE CountryId = 22;
UPDATE Country SET NameEsp = N'Brasil', Indicative = '55', Flag = 'https://flagcdn.com/br.svg' WHERE CountryId = 23;
UPDATE Country SET NameEsp = N'Brunéi', Indicative = '673', Flag = 'https://flagcdn.com/bn.svg' WHERE CountryId = 24;
UPDATE Country SET NameEsp = N'Bulgaria', Indicative = '359', Flag = 'https://flagcdn.com/bg.svg' WHERE CountryId = 25;
UPDATE Country SET NameEsp = N'Burkina Faso', Indicative = '226', Flag = 'https://flagcdn.com/bf.svg' WHERE CountryId = 26;
UPDATE Country SET NameEsp = N'Burundi', Indicative = '257', Flag = 'https://flagcdn.com/bi.svg' WHERE CountryId = 27;
UPDATE Country SET NameEsp = N'Camboya', Indicative = '855', Flag = 'https://flagcdn.com/kh.svg' WHERE CountryId = 28;
UPDATE Country SET NameEsp = N'Camerún', Indicative = '237', Flag = 'https://flagcdn.com/cm.svg' WHERE CountryId = 29;
UPDATE Country SET NameEsp = N'Canadá', Indicative = '1', Flag = 'https://flagcdn.com/ca.svg' WHERE CountryId = 30;
UPDATE Country SET NameEsp = N'Cabo Verde', Indicative = '238', Flag = 'https://flagcdn.com/cv.svg' WHERE CountryId = 31;
UPDATE Country SET NameEsp = N'República Centroafricana', Indicative = '236', Flag = 'https://flagcdn.com/cf.svg' WHERE CountryId = 32;
UPDATE Country SET NameEsp = N'Chad', Indicative = '235', Flag = 'https://flagcdn.com/td.svg' WHERE CountryId = 33;
UPDATE Country SET NameEsp = N'Chile', Indicative = '56', Flag = 'https://flagcdn.com/cl.svg' WHERE CountryId = 34;
UPDATE Country SET NameEsp = N'China', Indicative = '86', Flag = 'https://flagcdn.com/cn.svg' WHERE CountryId = 35;
UPDATE Country SET NameEsp = N'Colombia', Indicative = '57', Flag = 'https://flagcdn.com/co.svg' WHERE CountryId = 36;
UPDATE Country SET NameEsp = N'Comoras', Indicative = '269', Flag = 'https://flagcdn.com/km.svg' WHERE CountryId = 37;
UPDATE Country SET NameEsp = N'Congo', Indicative = '242', Flag = 'https://flagcdn.com/cg.svg' WHERE CountryId = 38;
UPDATE Country SET NameEsp = N'República Democrática del Congo', Indicative = '243', Flag = 'https://flagcdn.com/cd.svg' WHERE CountryId = 39;
UPDATE Country SET NameEsp = N'Costa Rica', Indicative = '506', Flag = 'https://flagcdn.com/cr.svg' WHERE CountryId = 40;
UPDATE Country SET NameEsp = N'Croacia', Indicative = '385', Flag = 'https://flagcdn.com/hr.svg' WHERE CountryId = 41;
UPDATE Country SET NameEsp = N'Cuba', Indicative = '53', Flag = 'https://flagcdn.com/cu.svg' WHERE CountryId = 42;
UPDATE Country SET NameEsp = N'Chipre', Indicative = '357', Flag = 'https://flagcdn.com/cy.svg' WHERE CountryId = 43;
UPDATE Country SET NameEsp = N'República Checa', Indicative = '420', Flag = 'https://flagcdn.com/cz.svg' WHERE CountryId = 44;
UPDATE Country SET NameEsp = N'Dinamarca', Indicative = '45', Flag = 'https://flagcdn.com/dk.svg' WHERE CountryId = 45;
UPDATE Country SET NameEsp = N'Yibuti', Indicative = '253', Flag = 'https://flagcdn.com/dj.svg' WHERE CountryId = 46;
UPDATE Country SET NameEsp = N'Dominica', Indicative = '1-767', Flag = 'https://flagcdn.com/dm.svg' WHERE CountryId = 47;
UPDATE Country SET NameEsp = N'República Dominicana', Indicative = '', Flag = 'https://flagcdn.com/do.svg' WHERE CountryId = 48;
UPDATE Country SET NameEsp = N'Ecuador', Indicative = '593', Flag = 'https://flagcdn.com/ec.svg' WHERE CountryId = 49;
UPDATE Country SET NameEsp = N'Egipto', Indicative = '20', Flag = 'https://flagcdn.com/eg.svg' WHERE CountryId = 50;

UPDATE Country SET NameEsp = N'El Salvador', Indicative = '503', Flag = 'https://flagcdn.com/sv.svg' WHERE CountryId = 51;
UPDATE Country SET NameEsp = N'Guinea Ecuatorial', Indicative = '240', Flag = 'https://flagcdn.com/gq.svg' WHERE CountryId = 52;
UPDATE Country SET NameEsp = N'Eritrea', Indicative = '291', Flag = 'https://flagcdn.com/er.svg' WHERE CountryId = 53;
UPDATE Country SET NameEsp = N'Estonia', Indicative = '372', Flag = 'https://flagcdn.com/ee.svg' WHERE CountryId = 54;
UPDATE Country SET NameEsp = N'Esuatini', Indicative = '268', Flag = 'https://flagcdn.com/sz.svg' WHERE CountryId = 55;
UPDATE Country SET NameEsp = N'Etiopía', Indicative = '251', Flag = 'https://flagcdn.com/et.svg' WHERE CountryId = 56;
UPDATE Country SET NameEsp = N'Fiyi', Indicative = '679', Flag = 'https://flagcdn.com/fj.svg' WHERE CountryId = 57;
UPDATE Country SET NameEsp = N'Finlandia', Indicative = '358', Flag = 'https://flagcdn.com/fi.svg' WHERE CountryId = 58;
UPDATE Country SET NameEsp = N'Francia', Indicative = '33', Flag = 'https://flagcdn.com/fr.svg' WHERE CountryId = 59;
UPDATE Country SET NameEsp = N'Gabón', Indicative = '241', Flag = 'https://flagcdn.com/ga.svg' WHERE CountryId = 60;
UPDATE Country SET NameEsp = N'Gambia', Indicative = '220', Flag = 'https://flagcdn.com/gm.svg' WHERE CountryId = 61;
UPDATE Country SET NameEsp = N'Georgia', Indicative = '995', Flag = 'https://flagcdn.com/ge.svg' WHERE CountryId = 62;
UPDATE Country SET NameEsp = N'Alemania', Indicative = '49', Flag = 'https://flagcdn.com/de.svg' WHERE CountryId = 63;
UPDATE Country SET NameEsp = N'Ghana', Indicative = '233', Flag = 'https://flagcdn.com/gh.svg' WHERE CountryId = 64;
UPDATE Country SET NameEsp = N'Grecia', Indicative = '30', Flag = 'https://flagcdn.com/gr.svg' WHERE CountryId = 65;
UPDATE Country SET NameEsp = N'Granada', Indicative = '1-473', Flag = 'https://flagcdn.com/gd.svg' WHERE CountryId = 66;
UPDATE Country SET NameEsp = N'Guatemala', Indicative = '502', Flag = 'https://flagcdn.com/gt.svg' WHERE CountryId = 67;
UPDATE Country SET NameEsp = N'Guinea', Indicative = '224', Flag = 'https://flagcdn.com/gn.svg' WHERE CountryId = 68;
UPDATE Country SET NameEsp = N'Guinea-Bisáu', Indicative = '245', Flag = 'https://flagcdn.com/gw.svg' WHERE CountryId = 69;
UPDATE Country SET NameEsp = N'Guyana', Indicative = '592', Flag = 'https://flagcdn.com/gy.svg' WHERE CountryId = 70;
UPDATE Country SET NameEsp = N'Haití', Indicative = '509', Flag = 'https://flagcdn.com/ht.svg' WHERE CountryId = 71;
UPDATE Country SET NameEsp = N'Honduras', Indicative = '504', Flag = 'https://flagcdn.com/hn.svg' WHERE CountryId = 72;
UPDATE Country SET NameEsp = N'Hungría', Indicative = '36', Flag = 'https://flagcdn.com/hu.svg' WHERE CountryId = 73;
UPDATE Country SET NameEsp = N'Islandia', Indicative = '354', Flag = 'https://flagcdn.com/is.svg' WHERE CountryId = 74;
UPDATE Country SET NameEsp = N'India', Indicative = '91', Flag = 'https://flagcdn.com/in.svg' WHERE CountryId = 75;
UPDATE Country SET NameEsp = N'Indonesia', Indicative = '62', Flag = 'https://flagcdn.com/id.svg' WHERE CountryId = 76;
UPDATE Country SET NameEsp = N'Irán', Indicative = '98', Flag = 'https://flagcdn.com/ir.svg' WHERE CountryId = 77;
UPDATE Country SET NameEsp = N'Irak', Indicative = '964', Flag = 'https://flagcdn.com/iq.svg' WHERE CountryId = 78;
UPDATE Country SET NameEsp = N'Irlanda', Indicative = '353', Flag = 'https://flagcdn.com/ie.svg' WHERE CountryId = 79;
UPDATE Country SET NameEsp = N'Israel', Indicative = '972', Flag = 'https://flagcdn.com/il.svg' WHERE CountryId = 80;
UPDATE Country SET NameEsp = N'Italia', Indicative = '39', Flag = 'https://flagcdn.com/it.svg' WHERE CountryId = 81;
UPDATE Country SET NameEsp = N'Costa de Marfil', Indicative = '225', Flag = 'https://flagcdn.com/ci.svg' WHERE CountryId = 82;
UPDATE Country SET NameEsp = N'Jamaica', Indicative = '', Flag = 'https://flagcdn.com/jm.svg' WHERE CountryId = 83;
UPDATE Country SET NameEsp = N'Japón', Indicative = '81', Flag = 'https://flagcdn.com/jp.svg' WHERE CountryId = 84;
UPDATE Country SET NameEsp = N'Jordania', Indicative = '962', Flag = 'https://flagcdn.com/jo.svg' WHERE CountryId = 85;
UPDATE Country SET NameEsp = N'Kazajistán', Indicative = '7', Flag = 'https://flagcdn.com/kz.svg' WHERE CountryId = 86;
UPDATE Country SET NameEsp = N'Kenia', Indicative = '254', Flag = 'https://flagcdn.com/ke.svg' WHERE CountryId = 87;
UPDATE Country SET NameEsp = N'Kiribati', Indicative = '686', Flag = 'https://flagcdn.com/ki.svg' WHERE CountryId = 88;
UPDATE Country SET NameEsp = N'Kuwait', Indicative = '965', Flag = 'https://flagcdn.com/kw.svg' WHERE CountryId = 89;
UPDATE Country SET NameEsp = N'Kirguistán', Indicative = '996', Flag = 'https://flagcdn.com/kg.svg' WHERE CountryId = 90;
UPDATE Country SET NameEsp = N'Laos', Indicative = '856', Flag = 'https://flagcdn.com/la.svg' WHERE CountryId = 91;
UPDATE Country SET NameEsp = N'Letonia', Indicative = '371', Flag = 'https://flagcdn.com/lv.svg' WHERE CountryId = 92;
UPDATE Country SET NameEsp = N'Líbano', Indicative = '961', Flag = 'https://flagcdn.com/lb.svg' WHERE CountryId = 93;
UPDATE Country SET NameEsp = N'Lesoto', Indicative = '266', Flag = 'https://flagcdn.com/ls.svg' WHERE CountryId = 94;
UPDATE Country SET NameEsp = N'Liberia', Indicative = '231', Flag = 'https://flagcdn.com/lr.svg' WHERE CountryId = 95;
UPDATE Country SET NameEsp = N'Libia', Indicative = '218', Flag = 'https://flagcdn.com/ly.svg' WHERE CountryId = 96;
UPDATE Country SET NameEsp = N'Liechtenstein', Indicative = '423', Flag = 'https://flagcdn.com/li.svg' WHERE CountryId = 97;
UPDATE Country SET NameEsp = N'Lituania', Indicative = '370', Flag = 'https://flagcdn.com/lt.svg' WHERE CountryId = 98;
UPDATE Country SET NameEsp = N'Luxemburgo', Indicative = '352', Flag = 'https://flagcdn.com/lu.svg' WHERE CountryId = 99;
UPDATE Country SET NameEsp = N'Madagascar', Indicative = '261', Flag = 'https://flagcdn.com/mg.svg' WHERE CountryId = 100;


UPDATE Country SET NameEsp = N'Malawi', Indicative = '265', Flag = 'https://flagcdn.com/mw.svg' WHERE CountryId = 101;
UPDATE Country SET NameEsp = N'Malasia', Indicative = '60', Flag = 'https://flagcdn.com/my.svg' WHERE CountryId = 102;
UPDATE Country SET NameEsp = N'Maldivas', Indicative = '960', Flag = 'https://flagcdn.com/mv.svg' WHERE CountryId = 103;
UPDATE Country SET NameEsp = N'Mali', Indicative = '223', Flag = 'https://flagcdn.com/ml.svg' WHERE CountryId = 104;
UPDATE Country SET NameEsp = N'Malta', Indicative = '356', Flag = 'https://flagcdn.com/mt.svg' WHERE CountryId = 105;
UPDATE Country SET NameEsp = N'Marshall', Indicative = '692', Flag = 'https://flagcdn.com/mh.svg' WHERE CountryId = 106;
UPDATE Country SET NameEsp = N'Mauritania', Indicative = '222', Flag = 'https://flagcdn.com/mr.svg' WHERE CountryId = 107;
UPDATE Country SET NameEsp = N'Mauricio', Indicative = '230', Flag = 'https://flagcdn.com/mu.svg' WHERE CountryId = 108;
UPDATE Country SET NameEsp = N'México', Indicative = '52', Flag = 'https://flagcdn.com/mx.svg' WHERE CountryId = 109;
UPDATE Country SET NameEsp = N'Micronesia', Indicative = '691', Flag = 'https://flagcdn.com/fm.svg' WHERE CountryId = 110;
UPDATE Country SET NameEsp = N'Moldavia', Indicative = '373', Flag = 'https://flagcdn.com/md.svg' WHERE CountryId = 111;
UPDATE Country SET NameEsp = N'Mónaco', Indicative = '377', Flag = 'https://flagcdn.com/mc.svg' WHERE CountryId = 112;
UPDATE Country SET NameEsp = N'Mongolia', Indicative = '976', Flag = 'https://flagcdn.com/mn.svg' WHERE CountryId = 113;
UPDATE Country SET NameEsp = N'Montenegro', Indicative = '382', Flag = 'https://flagcdn.com/me.svg' WHERE CountryId = 114;
UPDATE Country SET NameEsp = N'Marruecos', Indicative = '212', Flag = 'https://flagcdn.com/ma.svg' WHERE CountryId = 115;
UPDATE Country SET NameEsp = N'Mozambique', Indicative = '258', Flag = 'https://flagcdn.com/mz.svg' WHERE CountryId = 116;
UPDATE Country SET NameEsp = N'Myanmar', Indicative = '95', Flag = 'https://flagcdn.com/mm.svg' WHERE CountryId = 117;
UPDATE Country SET NameEsp = N'Namibia', Indicative = '264', Flag = 'https://flagcdn.com/na.svg' WHERE CountryId = 118;
UPDATE Country SET NameEsp = N'Nauru', Indicative = '674', Flag = 'https://flagcdn.com/nr.svg' WHERE CountryId = 119;
UPDATE Country SET NameEsp = N'Nepal', Indicative = '977', Flag = 'https://flagcdn.com/np.svg' WHERE CountryId = 120;
UPDATE Country SET NameEsp = N'Países Bajos', Indicative = '31', Flag = 'https://flagcdn.com/nl.svg' WHERE CountryId = 121;
UPDATE Country SET NameEsp = N'Nueva Zelanda', Indicative = '64', Flag = 'https://flagcdn.com/nz.svg' WHERE CountryId = 122;
UPDATE Country SET NameEsp = N'Nicaragua', Indicative = '505', Flag = 'https://flagcdn.com/ni.svg' WHERE CountryId = 123;
UPDATE Country SET NameEsp = N'Níger', Indicative = '227', Flag = 'https://flagcdn.com/ne.svg' WHERE CountryId = 124;
UPDATE Country SET NameEsp = N'Nigeria', Indicative = '234', Flag = 'https://flagcdn.com/ng.svg' WHERE CountryId = 125;
UPDATE Country SET NameEsp = N'Corea del Norte', Indicative = '850', Flag = 'https://flagcdn.com/kp.svg' WHERE CountryId = 126;
UPDATE Country SET NameEsp = N'Noruega', Indicative = '47', Flag = 'https://flagcdn.com/no.svg' WHERE CountryId = 127;
UPDATE Country SET NameEsp = N'Oman', Indicative = '968', Flag = 'https://flagcdn.com/om.svg' WHERE CountryId = 128;
UPDATE Country SET NameEsp = N'Pakistán', Indicative = '92', Flag = 'https://flagcdn.com/pk.svg' WHERE CountryId = 129;
UPDATE Country SET NameEsp = N'Palau', Indicative = '680', Flag = 'https://flagcdn.com/pw.svg' WHERE CountryId = 130;
UPDATE Country SET NameEsp = N'Palestina', Indicative = '970', Flag = 'https://flagcdn.com/ps.svg' WHERE CountryId = 131;
UPDATE Country SET NameEsp = N'Panamá', Indicative = '507', Flag = 'https://flagcdn.com/pa.svg' WHERE CountryId = 132;
UPDATE Country SET NameEsp = N'Papúa Nueva Guinea', Indicative = '675', Flag = 'https://flagcdn.com/pg.svg' WHERE CountryId = 133;
UPDATE Country SET NameEsp = N'Paraguay', Indicative = '595', Flag = 'https://flagcdn.com/py.svg' WHERE CountryId = 134;
UPDATE Country SET NameEsp = N'Perú', Indicative = '51', Flag = 'https://flagcdn.com/pe.svg' WHERE CountryId = 135;
UPDATE Country SET NameEsp = N'Filipinas', Indicative = '63', Flag = 'https://flagcdn.com/ph.svg' WHERE CountryId = 136;
UPDATE Country SET NameEsp = N'Polonia', Indicative = '48', Flag = 'https://flagcdn.com/pl.svg' WHERE CountryId = 137;
UPDATE Country SET NameEsp = N'Portugal', Indicative = '351', Flag = 'https://flagcdn.com/pt.svg' WHERE CountryId = 138;
UPDATE Country SET NameEsp = N'Qatar', Indicative = '974', Flag = 'https://flagcdn.com/qa.svg' WHERE CountryId = 139;
UPDATE Country SET NameEsp = N'República de Corea', Indicative = '82', Flag = 'https://flagcdn.com/kr.svg' WHERE CountryId = 140;
UPDATE Country SET NameEsp = N'República del Congo', Indicative = '242', Flag = 'https://flagcdn.com/cg.svg' WHERE CountryId = 141;
UPDATE Country SET NameEsp = N'República Democrática del Congo', Indicative = '243', Flag = 'https://flagcdn.com/cd.svg' WHERE CountryId = 142;
UPDATE Country SET NameEsp = N'Rumania', Indicative = '40', Flag = 'https://flagcdn.com/ro.svg' WHERE CountryId = 143;
UPDATE Country SET NameEsp = N'Rusia', Indicative = '7', Flag = 'https://flagcdn.com/ru.svg' WHERE CountryId = 144;
UPDATE Country SET NameEsp = N'Ruanda', Indicative = '250', Flag = 'https://flagcdn.com/rw.svg' WHERE CountryId = 145;
UPDATE Country SET NameEsp = N'San Cristóbal y Nieves', Indicative = '1-869', Flag = 'https://flagcdn.com/kn.svg' WHERE CountryId = 146;
UPDATE Country SET NameEsp = N'San Marino', Indicative = '378', Flag = 'https://flagcdn.com/sm.svg' WHERE CountryId = 147;
UPDATE Country SET NameEsp = N'San Vicente y las Granadinas', Indicative = '1-784', Flag = 'https://flagcdn.com/vc.svg' WHERE CountryId = 148;
UPDATE Country SET NameEsp = N'Santa Lucía', Indicative = '1-758', Flag = 'https://flagcdn.com/lc.svg' WHERE CountryId = 149;
UPDATE Country SET NameEsp = N'Samoa', Indicative = '685', Flag = 'https://flagcdn.com/ws.svg' WHERE CountryId = 150;


UPDATE Country SET NameEsp = N'San Cristóbal y Nieves', Indicative = '1-869', Flag = 'https://flagcdn.com/kn.svg' WHERE CountryId = 151;
UPDATE Country SET NameEsp = N'San Marino', Indicative = '378', Flag = 'https://flagcdn.com/sm.svg' WHERE CountryId = 152;
UPDATE Country SET NameEsp = N'San Vicente y las Granadinas', Indicative = '1-784', Flag = 'https://flagcdn.com/vc.svg' WHERE CountryId = 153;
UPDATE Country SET NameEsp = N'Santa Lucía', Indicative = '1-758', Flag = 'https://flagcdn.com/lc.svg' WHERE CountryId = 154;
UPDATE Country SET NameEsp = N'Samoa', Indicative = '685', Flag = 'https://flagcdn.com/ws.svg' WHERE CountryId = 155;
UPDATE Country SET NameEsp = N'Santo Tomé y Príncipe', Indicative = '239', Flag = 'https://flagcdn.com/st.svg' WHERE CountryId = 156;
UPDATE Country SET NameEsp = N'Senegal', Indicative = '221', Flag = 'https://flagcdn.com/sn.svg' WHERE CountryId = 157;
UPDATE Country SET NameEsp = N'Serbia', Indicative = '381', Flag = 'https://flagcdn.com/rs.svg' WHERE CountryId = 158;
UPDATE Country SET NameEsp = N'Seychelles', Indicative = '248', Flag = 'https://flagcdn.com/sc.svg' WHERE CountryId = 159;
UPDATE Country SET NameEsp = N'Sierra Leona', Indicative = '232', Flag = 'https://flagcdn.com/sl.svg' WHERE CountryId = 160;
UPDATE Country SET NameEsp = N'Singapur', Indicative = '65', Flag = 'https://flagcdn.com/sg.svg' WHERE CountryId = 161;
UPDATE Country SET NameEsp = N'Siria', Indicative = '963', Flag = 'https://flagcdn.com/sy.svg' WHERE CountryId = 162;
UPDATE Country SET NameEsp = N'Somalia', Indicative = '252', Flag = 'https://flagcdn.com/so.svg' WHERE CountryId = 163;
UPDATE Country SET NameEsp = N'Sri Lanka', Indicative = '94', Flag = 'https://flagcdn.com/lk.svg' WHERE CountryId = 164;
UPDATE Country SET NameEsp = N'Sudáfrica', Indicative = '27', Flag = 'https://flagcdn.com/za.svg' WHERE CountryId = 165;
UPDATE Country SET NameEsp = N'Sudán', Indicative = '249', Flag = 'https://flagcdn.com/sd.svg' WHERE CountryId = 166;
UPDATE Country SET NameEsp = N'Sudán del Sur', Indicative = '211', Flag = 'https://flagcdn.com/ss.svg' WHERE CountryId = 167;
UPDATE Country SET NameEsp = N'Suecia', Indicative = '46', Flag = 'https://flagcdn.com/se.svg' WHERE CountryId = 168;
UPDATE Country SET NameEsp = N'Suiza', Indicative = '41', Flag = 'https://flagcdn.com/ch.svg' WHERE CountryId = 169;
UPDATE Country SET NameEsp = N'Surinam', Indicative = '597', Flag = 'https://flagcdn.com/sr.svg' WHERE CountryId = 170;
UPDATE Country SET NameEsp = N'Swazilandia', Indicative = '268', Flag = 'https://flagcdn.com/sz.svg' WHERE CountryId = 171;
UPDATE Country SET NameEsp = N'Tailandia', Indicative = '66', Flag = 'https://flagcdn.com/th.svg' WHERE CountryId = 172;
UPDATE Country SET NameEsp = N'Taiwán', Indicative = '886', Flag = 'https://flagcdn.com/tw.svg' WHERE CountryId = 173;
UPDATE Country SET NameEsp = N'Tanzania', Indicative = '255', Flag = 'https://flagcdn.com/tz.svg' WHERE CountryId = 174;
UPDATE Country SET NameEsp = N'Tayikistán', Indicative = '992', Flag = 'https://flagcdn.com/tj.svg' WHERE CountryId = 175;
UPDATE Country SET NameEsp = N'Timor Oriental', Indicative = '670', Flag = 'https://flagcdn.com/tl.svg' WHERE CountryId = 176;
UPDATE Country SET NameEsp = N'Togo', Indicative = '228', Flag = 'https://flagcdn.com/tg.svg' WHERE CountryId = 177;
UPDATE Country SET NameEsp = N'Tonga', Indicative = '676', Flag = 'https://flagcdn.com/to.svg' WHERE CountryId = 178;
UPDATE Country SET NameEsp = N'Trinidad y Tobago', Indicative = '1-868', Flag = 'https://flagcdn.com/tt.svg' WHERE CountryId = 179;
UPDATE Country SET NameEsp = N'Túnez', Indicative = '216', Flag = 'https://flagcdn.com/tn.svg' WHERE CountryId = 180;
UPDATE Country SET NameEsp = N'Turquía', Indicative = '90', Flag = 'https://flagcdn.com/tr.svg' WHERE CountryId = 181;
UPDATE Country SET NameEsp = N'Turkmenistán', Indicative = '993', Flag = 'https://flagcdn.com/tm.svg' WHERE CountryId = 182;
UPDATE Country SET NameEsp = N'Tuvalu', Indicative = '688', Flag = 'https://flagcdn.com/tv.svg' WHERE CountryId = 183;
UPDATE Country SET NameEsp = N'Uganda', Indicative = '256', Flag = 'https://flagcdn.com/ug.svg' WHERE CountryId = 184;
UPDATE Country SET NameEsp = N'Ucrania', Indicative = '380', Flag = 'https://flagcdn.com/ua.svg' WHERE CountryId = 185;
UPDATE Country SET NameEsp = N'Emiratos Árabes Unidos', Indicative = '971', Flag = 'https://flagcdn.com/ae.svg' WHERE CountryId = 186;
UPDATE Country SET NameEsp = N'Reino Unido', Indicative = '44', Flag = 'https://flagcdn.com/gb.svg' WHERE CountryId = 187;
UPDATE Country SET NameEsp = N'Estados Unidos', Indicative = '1', Flag = 'https://flagcdn.com/us.svg' WHERE CountryId = 188;
UPDATE Country SET NameEsp = N'Uruguay', Indicative = '598', Flag = 'https://flagcdn.com/uy.svg' WHERE CountryId = 189;
UPDATE Country SET NameEsp = N'Uzbekistán', Indicative = '998', Flag = 'https://flagcdn.com/uz.svg' WHERE CountryId = 190;
UPDATE Country SET NameEsp = N'Vanuatu', Indicative = '678', Flag = 'https://flagcdn.com/vu.svg' WHERE CountryId = 191;
UPDATE Country SET NameEsp = N'Vaticano', Indicative = '379', Flag = 'https://flagcdn.com/va.svg' WHERE CountryId = 192;
UPDATE Country SET NameEsp = N'Venezuela', Indicative = '58', Flag = 'https://flagcdn.com/ve.svg' WHERE CountryId = 193;
UPDATE Country SET NameEsp = N'Vietnam', Indicative = '84', Flag = 'https://flagcdn.com/vn.svg' WHERE CountryId = 194;




update Country set NameESP = 'Malaui' where CountryId = 101;
update Country set NameESP = 'Islas Marshall' where CountryId = 106;
update Country set NameESP = 'Rumania' where CountryId = 140;
update Country set NameESP = 'Rusia' where CountryId = 141;
update Country set NameESP = 'Ruanda' where CountryId = 142;
update Country set NameESP = 'San Cristóbal y Nieves' where CountryId = 143;
update Country set NameESP = 'Santa Lucía' where CountryId = 144;
update Country set NameESP = 'San Vicente y las Granadinas' where CountryId = 145;
update Country set NameESP = 'Samoa' where CountryId = 146;
update Country set NameESP = 'Santo Tomé y Príncipe' where CountryId = 148;
update Country set NameESP = 'Arabia Saudita' where CountryId = 149;
update Country set NameESP = 'Senegal' where CountryId = 150;
update Country set NameESP = 'Serbia' where CountryId = 151;
update Country set NameESP = 'Seychelles' where CountryId = 152;
update Country set NameESP = 'Sierra Leona' where CountryId = 153;
update Country set NameESP = 'Singapur' where CountryId = 154;
update Country set NameESP = 'Eslovaquia' where CountryId = 155;
update Country set NameESP = 'Eslovenia' where CountryId = 156;
update Country set NameESP = 'Islas Salomon' where CountryId = 157;
update Country set NameESP = 'Somalia' where CountryId = 158;
update Country set NameESP = 'Sudafrica' where CountryId = 159;
update Country set NameESP = 'Corea del sur' where CountryId = 160;
update Country set NameESP = 'Sudan del sur' where CountryId = 161;
update Country set NameESP = 'España' where CountryId = 162;
update Country set NameESP = 'Sri Lanka' where CountryId = 163
update Country set NameESP = 'Sudan' where CountryId = 164;
update Country set NameESP = 'Surinam' where CountryId = 165;
update Country set NameESP = 'Suecia' where CountryId = 166;
update Country set NameESP = 'Suiza' where CountryId = 167;
update Country set NameESP = 'Siria' where CountryId = 168;
update Country set NameESP = 'Taiwan' where CountryId = 169;
update Country set NameESP = 'Tayikistan' where CountryId = 170;
update Country set NameESP = 'Tanzania' where CountryId = 171;
update Country set NameESP = 'Tailandia' where CountryId = 172;
update Country set NameESP = 'Timor Oriental' where CountryId = 173;
update Country set NameESP = 'Togo' where CountryId = 174;
update Country set NameESP = 'Tonga' where CountryId = 175;
update Country set NameESP = 'Trinidad y Tobago' where CountryId = 176;
update Country set NameESP = 'Tunez' where CountryId = 177;
update Country set NameESP = 'Turquia' where CountryId = 178;
update Country set NameESP = 'Turkmenistan' where CountryId = 179;
update Country set NameESP = 'Tuvalu' where CountryId = 180;
update Country set NameESP = 'Uganda' where CountryId = 181;
update Country set NameESP = 'Ucrania' where CountryId = 182;
update Country set NameESP = 'Emiratos Arabes Unidos' where CountryId = 183;
update Country set NameESP = 'Reino Unido' where CountryId = 184;
update Country set NameESP = 'Estados Unidos' where CountryId = 185;
update Country set NameESP = 'Uruguay' where CountryId = 186;
update Country set NameESP = 'Uzbekistan' where CountryId = 187;
update Country set NameESP = 'Vanuatu' where CountryId = 188;
update Country set NameESP = 'Vaticano' where CountryId = 189;
update Country set NameESP = 'Venezuela' where CountryId = 190;
update Country set NameESP = 'Vietnam' where CountryId = 191;
update Country set NameESP = 'Yemen' where CountryId = 192;
update Country set NameESP = 'Zambia' where CountryId = 193;
update Country set NameESP = 'Zimbabue' where CountryId = 194;
update Country set NameESP = 'Macedonia' where CountryId = 127;
update Country set NameESP = 'Noruega' where CountryId = 128;
update Country set NameESP = 'Oman' where CountryId = 129;
update Country set NameESP = 'Pakistan' where CountryId = 130;
update Country set NameESP = 'Palau' where CountryId = 131;




update COUNTRY SET Indicative='389', FLAG= 'https://flagcdn.com/mk.svg' WHERE CountryId = 127 --Macedonia
update COUNTRY SET Indicative='47', FLAG= 'https://flagcdn.com/no.svg' WHERE CountryId = 128 --Noruega
update COUNTRY SET Indicative='968', FLAG= 'https://flagcdn.com/om.svg' WHERE CountryId = 129 --Oman
update COUNTRY SET Indicative='92', FLAG= 'https://flagcdn.com/pk.svg' WHERE CountryId = 130 --Pakistan
update COUNTRY SET Indicative='680', FLAG= 'https://flagcdn.com/pw.svg' WHERE CountryId = 131 --Palau
update COUNTRY SET Indicative='507', FLAG= 'https://flagcdn.com/pa.svg' WHERE CountryId = 132 --Panama
update COUNTRY SET Indicative='40', FLAG= 'https://flagcdn.com/ro.svg' WHERE CountryId = 140 --Rumania
update COUNTRY SET Indicative='7', FLAG= 'https://flagcdn.com/ru.svg' WHERE CountryId = 141 --Rusia
update COUNTRY SET Indicative='250', FLAG= 'https://flagcdn.com/rw.svg' WHERE CountryId = 142 --Rwanda
update COUNTRY SET Indicative='1-869', FLAG= 'https://flagcdn.com/kn.svg' WHERE CountryId = 143 --SanCristobalNieves
update COUNTRY SET Indicative='1-758', FLAG= 'https://flagcdn.com/lc.svg' WHERE CountryId = 144 --SantaLucia
update COUNTRY SET Indicative='1-784', FLAG= 'https://flagcdn.com/vc.svg' WHERE CountryId = 145 --SanVicenteGranadinas
update COUNTRY SET Indicative='685', FLAG= 'https://flagcdn.com/ws.svg' WHERE CountryId = 146 --Samoa
update COUNTRY SET Indicative='378', FLAG= 'https://flagcdn.com/sm.svg' WHERE CountryId = 147 --SanMarino
update COUNTRY SET Indicative='239', FLAG= 'https://flagcdn.com/st.svg' WHERE CountryId = 148 --SantoTomePrincipe
update COUNTRY SET Indicative='966', FLAG= 'https://flagcdn.com/sa.svg' WHERE CountryId = 149 --ArabiaSaudita
update COUNTRY SET Indicative='221', FLAG= 'https://flagcdn.com/sn.svg' WHERE CountryId = 150 --Senegal
update COUNTRY SET Indicative='381', FLAG= 'https://flagcdn.com/rs.svg' WHERE CountryId = 151 --Serbia
update COUNTRY SET Indicative='248', FLAG= 'https://flagcdn.com/sc.svg' WHERE CountryId = 152 --Seychelles
update COUNTRY SET Indicative='232', FLAG= 'https://flagcdn.com/sl.svg' WHERE CountryId = 153 --SierraLeona
update COUNTRY SET Indicative='65', FLAG= 'https://flagcdn.com/sg.svg' WHERE CountryId = 154 --Singapur
update COUNTRY SET Indicative='421', FLAG= 'https://flagcdn.com/sk.svg' WHERE CountryId = 155 --Eslovaquia
update COUNTRY SET Indicative='386', FLAG= 'https://flagcdn.com/si.svg' WHERE CountryId = 156 --Eslovenia
update COUNTRY SET Indicative='677', FLAG= 'https://flagcdn.com/sb.svg' WHERE CountryId = 157 --IslasSalomon
update COUNTRY SET Indicative='252', FLAG= 'https://flagcdn.com/so.svg' WHERE CountryId = 158 --Somalia
update COUNTRY SET Indicative='27', FLAG= 'https://flagcdn.com/za.svg' WHERE CountryId = 159 --Sudafrica
update COUNTRY SET Indicative='82', FLAG= 'https://flagcdn.com/kr.svg' WHERE CountryId = 160 --CoreaSur
update COUNTRY SET Indicative='211', FLAG= 'https://flagcdn.com/ss.svg' WHERE CountryId = 161 --SudanSur
update COUNTRY SET Indicative='34', FLAG= 'https://flagcdn.com/es.svg' WHERE CountryId = 162 --España
update COUNTRY SET Indicative='94', FLAG= 'https://flagcdn.com/lk.svg' WHERE CountryId = 163 --SriLanka
update COUNTRY SET Indicative='249', FLAG= 'https://flagcdn.com/sd.svg' WHERE CountryId = 164 --Sudan
update COUNTRY SET Indicative='597', FLAG= 'https://flagcdn.com/sr.svg' WHERE CountryId = 165 --Suriname
update COUNTRY SET Indicative='46', FLAG= 'https://flagcdn.com/se.svg' WHERE CountryId = 166 --Suecia
update COUNTRY SET Indicative='41', FLAG= 'https://flagcdn.com/ch.svg' WHERE CountryId = 167 --Suiza
update COUNTRY SET Indicative='963', FLAG= 'https://flagcdn.com/sy.svg' WHERE CountryId = 168 --Siria
update COUNTRY SET Indicative='886', FLAG= 'https://flagcdn.com/tw.svg' WHERE CountryId = 169 --Taiwan
update COUNTRY SET Indicative='992', FLAG= 'https://flagcdn.com/tj.svg' WHERE CountryId = 170 --Tayikistan
update COUNTRY SET Indicative='255', FLAG= 'https://flagcdn.com/tz.svg' WHERE CountryId = 171 --Tanzania
update COUNTRY SET Indicative='66', FLAG= 'https://flagcdn.com/th.svg' WHERE CountryId = 172 --Tailandia
update COUNTRY SET Indicative='670', FLAG= 'https://flagcdn.com/tl.svg' WHERE CountryId = 173 --TimorOriental
update COUNTRY SET Indicative='228', FLAG= 'https://flagcdn.com/tg.svg' WHERE CountryId = 174 --Togo
update COUNTRY SET Indicative='676', FLAG= 'https://flagcdn.com/to.svg' WHERE CountryId = 175 --Tonga
update COUNTRY SET Indicative='1-868', FLAG= 'https://flagcdn.com/tt.svg' WHERE CountryId = 176 --TrinidadTobago
update COUNTRY SET Indicative='216', FLAG= 'https://flagcdn.com/tn.svg' WHERE CountryId = 177 --Tunez
update COUNTRY SET Indicative='90', FLAG= 'https://flagcdn.com/tr.svg' WHERE CountryId = 178 --Turquia
update COUNTRY SET Indicative='993', FLAG= 'https://flagcdn.com/tm.svg' WHERE CountryId = 179 --Turkmenistan
update COUNTRY SET Indicative='688', FLAG= 'https://flagcdn.com/tv.svg' WHERE CountryId = 180 --Tuvalu
update COUNTRY SET Indicative='256', FLAG= 'https://flagcdn.com/ug.svg' WHERE CountryId = 181 --Uganda
update COUNTRY SET Indicative='380', FLAG= 'https://flagcdn.com/ua.svg' WHERE CountryId = 182 --Ucrania
update COUNTRY SET Indicative='971', FLAG= 'https://flagcdn.com/ae.svg' WHERE CountryId = 183 --EmiratosArabes
update COUNTRY SET Indicative='44', FLAG= 'https://flagcdn.com/gb.svg' WHERE CountryId = 184 --GranBretaña
update COUNTRY SET Indicative='1', FLAG= 'https://flagcdn.com/us.svg' WHERE CountryId = 185 --USA
update COUNTRY SET Indicative='598', FLAG= 'https://flagcdn.com/uy.svg' WHERE CountryId = 186 --Uruguay
update COUNTRY SET Indicative='998', FLAG= 'https://flagcdn.com/uz.svg' WHERE CountryId = 187 --Uzbekistan
update COUNTRY SET Indicative='678', FLAG= 'https://flagcdn.com/vu.svg' WHERE CountryId = 188 --Vanuatu
update COUNTRY SET Indicative='379', FLAG= 'https://flagcdn.com/va.svg' WHERE CountryId = 189 --Vaticano
update COUNTRY SET Indicative='58', FLAG= 'https://flagcdn.com/ve.svg' WHERE CountryId = 190 --Venezuela
update COUNTRY SET Indicative='84', FLAG= 'https://flagcdn.com/vn.svg' WHERE CountryId = 191 --Vietnam
update COUNTRY SET Indicative='967', FLAG= 'https://flagcdn.com/ye.svg' WHERE CountryId = 192 --Yemen
update COUNTRY SET Indicative='260', FLAG= 'https://flagcdn.com/zm.svg' WHERE CountryId = 193 --Zambia
update COUNTRY SET Indicative='263', FLAG= 'https://flagcdn.com/zw.svg' WHERE CountryId = 194 --Zimbabue

            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                update country set Indicative= NULL, flag= '', nameEsp= '' ; -- region por defecto
            ");
        }

    }
}
