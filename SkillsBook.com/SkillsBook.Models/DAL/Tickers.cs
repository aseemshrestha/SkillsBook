using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsBook.Models.DAL
{
    public class Tickers
    {
        private String fromA1 = "aau,aame,abac,abcd,abev,abio,abus,acfn,achn,acpw,acrx,acst,acts,acur,adap,adat,adge,adhd,adk,admp";
        private String fromA2 = "advm,aegr,aehr,aemd,aeti,aey,aezs,afmd,agen,agfs,ahpi,aiii,airi,aixg,aker,akg,alim,allt,aln,alqa";
        private String fromA3 = "alsk,amcf,amda,ampe,amcn,ampe,amrn,amrs,ams,amtx,anth,any,apdn,aphb,apps,appy,apri,apt,apwc,arc";
        private String fromA4 = "arci,arcw,ardm,arex,arna,args,arna,arql,artw,artx,arwr,asg,asm,aspn,asrv,ast,astc,at,atai,atea";
        private String fromA5 = "atec,athx,atlc,atls,atnm,atos,atrm,atrs,aumn,auo,auph,auy,aveo,avgr,avid,avxl,awx,axas,axn,axti";
        private String fromA6 = "axu,azur";


        private String fromB1 = "baa,bas,basi,bbrg,bcei,blci,bdr,bdsi,bebe,bgi,bgmd,bioc,biol,bios,bldp,blfs,blin,blph,blrx,bmra";
        private String fromB2 = "bnso,bntc,bont,born,bosc,bpmx,bpth,brn,bspm,bstg,bte,btg,btx,bur,bv,bvx,bwen,bxe,byfc";



        private String fromC1 = "cach,cadc,cafn,cala,cali,camt,canf,capn,capr,carv,cas,casi,casm,catb,caw,cbak,cbay,cbio,cbk,cbli";
        private String fromC2 = "cbmx,cbr,cccl,cccr,ccih,ccm,ccih,ccm,cdna,cdor,cdti,cdxc,cdxs,cerc,ceru,cetc,cetv,cetx,cfbk,cfrx";
        private String fromC3 = "cga,cgix,cgnt,chci,chek,chgs,chkr,chma,chnr,cidm,cie,cif,cig,cjjd,clbs,cldx,clir,clmt,clne,clnt";
        private String fromC4 = "clrb,clsn,club,clwt,cmls,cmrx,cmu,cnat,cnet,cnit,cntf,cnxr,co,cogt,covs,cool,coyn,cpah,cphi,cphr";
        private String fromC5 = "cplp,cprx,cpsh,cpsl,cpst,creg,cris,crmd,crme,crnt,crtn,crnt,crvp,csbr,ctg,cslt,cthr,ctso,ctic,ctrv";
        private String fromC6 = "ctso,cur,cveo,cvm,cvrs,cxdc,cxe,cxrx,cvyan,cycc,cyrn,cyrx";

        private String fromD1 = "dac,dag,daio,dca,dcix,dcth,dde,delt,dfbg,dgaz,dgse,dhf,dht,dhy,dmpi,dmtx,dnai,dnn,dnr,dprx";
        private String fromD2 = "dpw,drad,dram,drd,drio,drna,drrx,drwi,drys,dsci,dss,dswl,dsx,dvd,dxlg,dxyn,dynt,dyy";


        private String fromE1 = "ears,ebio,ecr,ect,ecyt,edap,edmc,egan,edi,ego,egt,egy,ekso,elmd,elon,else,eltk,eltk,eman,emg";
        private String fromE2 = "emitf,emms,emxx,eng,enph,enrj,ensv,enzn,eqs,ern,esea,eses,esmc,esnc,essx,etrm,evar,evep,evk,evlv";
        private String fromE3 = "evok,evol,evri,exfo,exk,extr,eyeg,eyes";

        private String fromF1 = "fac,falc,fate,fax,fbio,fcel,fcsc,feng,fenx,ffhl,fh,fhco,fll,fncx,fnjn,ford,fpp,frsh,fsi,fsnn";
        private String fromF2 = "ftek,ftr,fuel,fve";

        private String fromG1 = "gale,galt,gass,gaz,gbim,gbr,gbsn,gcv,gen,gene,gern,gevo,gfa,gfi,gfn,ggb,gig,giga,gigm,glbl";
        private String fromG2 = "glch,gldd,glf,glmd,glow,glpw,glri,gluu,gman,gmo,gnca,gnrt,gnvc,gnw,gogl,goro,gpl,gprk,grn,grow";
        private String fromG3 = "grpn,gru,gsat,gsb,gsl,gst,gsv,gsvc,gte,gtim,gure,gv,gvp";

        private String fromH1 = "hbio,hear,heb,hgg,hgsh,hgt,hh,hhs,hiho,hil,hio,hlth,hmny,hmy,hotr,hov,hpj,hrt,hsgx,hson";
        private String fromH2 = "htbx,htgm,htm,husa,hygs";

        private String fromI1 = "i,iag,ibio,icad,icl,icld,idn,idra,idsa,idsy,idxg,iec,ifmi,ifon,igc,iht,iii,ikgh,imgn,imi";
        private String fromI2 = "immu,immy,imn,imnp,imuc,inap,infi,infu,innl,inod,ins,intt,intx,inuv,inve,invt,iots,ipas,ipci,ipi";
        private String fromI3 = "ipwr,irg,isig,isns,isr,issc,iti";

        private String fromJ = "jagx,jasn,jcs,jive,jmei,job,jone,jrjc,jtpy,jynt";

        private String fromK = "kbio,kcap,kgc,kgji,kin,kiq,kmph,kone,kool,kopn,koss,ktov";

        private String fromL1 = "lbix,leds,lee,lei,leju,lens,lgcy,life,lime,linc,liqt,litb,live,llex,llnw,lnd,lode,lov,lpcn,lpth";
        private String fromL2 = "lrad,lbtr,ltea,ltre,ltrx,lts,lub,luna,lyg";

        private String fromM1 = "mara,mark,marps,matn,matn,mbii,mblx,mbrx,mcep,mcur,mcz,mdgs,mdsy,mdvx,meet,meil,meip,memp,mfcb,mfg";
        private String fromM2 = "mgh,mhgc,mict,min,mind,mirn,mlnk,mlss,mndo,mnga,mnkd,mobl,moc,mrin,mosy,mdsi,msli,msn,mstx,mtbc";
        private String fromM3 = "mtsl,mux,mvis,mxc,mxwl,myos";

        private String fromN1 = "nadl,nak,nao,nauh,navb,nbrv,nby,nct,ncty,ndls,ndro,neon,neot,nept,nete,nfec,ng,ngd,nge,nhld";
        private String fromN2 = "nihd,nlst,nm,nmm,nna,nnvc,noa,nok,nq,nspr,nsu,nsys,ntip,ntz,nuro,nvax,nvcn,nvet,nvfy,nvgn";
        private String fromN3 = "nviv,nvls,nwbo,nwy,nxtd,nymx";

        private String fromO1 = "oasm,obci,occ,ocls,ocrx,odp,oesx,ogen,ogxi,ohai,ohgi,ohrp,oibr-c,oiim,olo,ontx,onvi,opgn,ophc,optt";
        private String fromO2 = "opxa,orbt,orex,orig,orpn,osg,osn,otiv,osasm,ozm";

        private String fromP1 = "pacd,panl,pbib,pbmd,pbw,pdex,pdli,ped,per,perf,peri,pesi,pfie,pfmt,pgh,pglc,phmd,pim";
        private String fromP2 = "pip,pkd,plg,plm,plpm,plug,plx,pme,pmts,pphm,ppp,ppt,pq,pran,prkr,prph,prqr,prss,prts,psdv";
        private String fromP3 = "ptie,ptn,ptsx,ptx,pulm,pvct,pwe,pxlw,pyds,pzg,pzrx";

        private String fromQ = "qbak,qcco,qkls,qlti,qnst,qrhc,qtm,quik,qumu";

        private String fromR1 = "rada,ras,rand,rave,rbcn,rbpaa,rcg,rcon,reed,refr,relv,renn,resn,rexx,rfil,rfp,rgls,rgse,ribt,ritt";
        private String fromR2 = "rjn,rkda,rlgt,rlog,rmgn,rnn,rnva,rnwk,roia,roiak,roka,rosg,rox,royl,royt,rprx,rsys,rt,rtix,rtk";
        private String fromR3 = "rttr,rvp,rwlk,rxii";

        private String fromS1 = "sajam,san,sand,sanw,sauc,sb,sbbp,sbot,sbsa,sckt,scon,scyx,sdlp,sdr,sdrl,sdt,seac,seed,sfun,sgm";
        private String fromS2 = "sgmo,sgoc,sgrp,ship,sid,sieb,sify,siga,sino,siri,sito,sixd,skbi,skis,skln,skys,smed,smit,smlr,smsi";
        private String fromS3 = "smtx,snmx,snss,sofo,sol,sorl,spcb,spdc,spex,sphs,sppi,sprt,sqns,srsc,ssh,sskn,ssn,ssy,staf";
        private String fromS4 = "stdy,stem,stks,stly,stng,stri,stv,stxs,sumr,sunw,subl,svu,sxe,symx,syn,sync,sypr,syrx";

        private String fromT1 = "tait,tanh,tat,tbio,tcco,tdw,tear,tenx,tga,tgb,tgc,tgd,tgen,thld,thm,thst,tik,tisa,tkai,tlog";
        private String fromT2 = "tmq,tndm,tnk,tnp,tnxp,tops,tplm,trch,trmr,trov,trq,trt,trx,trxc,tst,ttnp,ttph,tvia,twer,twmc";

        private String fromU = "tyht,uamy,uec,ulbi,ultr,umc,unis,unxl,uqm,urg,urre,usat,uti,utsi,uuu,uuuu,uwn";

        private String fromV1 = "vbiv,vcel,vgz,vhc,vhi,vicl,vii,vip,virc,vjet,vktx,vltc,vmem,vnce,vnr,vnrx,voc,voxx,vpco,vray";
        private String fromV2 = "vrml,vrtb,vslr,vsr,vstm,vtgn,vtnr,vvr,vvus";

        private String fromW = "wg,wga,wgbs,whlr,wiln,wmih,wpcs,wprt,wrn,wsci,wstl,wti,wtt,wyy";

        private String fromX = "xbks,xco,xelb,xgti,xnet,xny,xpl,xplr,xra,xtlb,xxii";

        private String fromY = "yeco,yge,yuma,yume";

        private String fromZ = "zais,zaza,zdge,zfgn,zixi,zn,znga,zsan,zx";

        public String getBatch1X()
        {
            return fromX;
        }
        public String getBatch1Y()
        {
            return fromY;
        }
        public String getBatch1T()
        {
            return fromT1;
        }
        public String getBatch2T()
        {
            return fromT2;
        }
        public String getBatch1U()
        {
            return fromU;
        }
        public String getBatch1V()
        {
            return fromV1;
        }
        public String getBatch2V()
        {
            return fromV2;
        }
        public String getBatch1W()
        {
            return fromW;
        }
        
      
        public String getBatch1Z()
        {
            return fromZ;
        }
       
        public String getBatch1Q()
        {
            return fromQ;
        }
        public String getBatch1R()
        {
            return fromR1;
        }
        public String getBatch2R()
        {
            return fromR2;
        }
        public String getBatch3R()
        {
            return fromR3;
        }
        public String getBatch1S()
        {
            return fromS1;
        }
        public String getBatch2S()
        {
            return fromS2;
        }
        public String getBatch3S()
        {
            return fromS3;
        }
        public String getBatch4S()
        {
            return fromS4;
        }
        public String getBatch1O()
        {
            return fromO1;
        }
        public String getBatch2O()
        {
            return fromO2;
        }
        public String getBatch1P()
        {
            return fromP1;
        }
        public String getBatch2P()
        {
            return fromP2;
        }
        public String getBatch3P()
        {
            return fromP3;
        }
        public String getBatch1K()
        {
            return fromK;
        }
        public String getBatch1L()
        {
            return fromL1;
        }
        public String getBatch2L()
        {
            return fromL2;
        }
        public String getBatch1M()
        {
            return fromM1;
        }
        public String getBatch2M()
        {
            return fromM2;
        }
        public String getBatch3M()
        {
            return fromM3;
        }
        public String getBatch1N()
        {
            return fromN1;
        }
        public String getBatch2N()
        {
            return fromN2;
        }
        public String getBatch3N()
        {
            return fromN3;
        }
        public String getBatch1H()
        {
            return fromH1;
        }
        public String getBatch2H()
        {
            return fromH2;
        }
        public String getBatch1I()
        {
            return fromI1;
        }
        public String getBatch2I()
        {
            return fromI2;
        }
        public String getBatch3I()
        {
            return fromI3;
        }
        public String getBatch1J()
        {
            return fromJ;
        }
        public String getBatch1F()
        {
            return fromF1;
        }
        public String getBatch2F()
        {
            return fromF2;
        }
        public String getBatch1G()
        {
            return fromG1;
        }
        public String getBatch2G()
        {
            return fromG2;
        }
        public String getBatch3G()
        {
            return fromG3;
        }
        public String getBatch1D()
        {
            return fromD1;
        }
        public String getBatch2D()
        {
            return fromD2;
        }
        public String getBatch1E()
        {
            return fromE1;
        }
        public String getBatch2E()
        {
            return fromE2;
        }
        public String getBatch3E()
        {
            return fromE3;
        }
        public String getBatch1C()
        {
            return fromC1;
        }
        public String getBatch2C()
        {
            return fromC2;
        }
        public String getBatch3C()
        {
            return fromC3;
        }
        public String getBatch4C()
        {
            return fromC4;
        }
        public String getBatch5C()
        {
            return fromC5;
        }
        public String getBatch6C()
        {
            return fromC6;
        }
        public String getBatch1B()
        {
            return fromB1;
        }
        public String getBatch2B()
        {
            return fromB2;
        }
        public String getBatch1A()
        {
            return fromA1;
        }
        public String getBatch2A()
        {
            return fromA2;
        }
        public String getBatch3A()
        {
            return fromA3;
        }
        public String getBatch4A()
        {
            return fromA4;
        }
        public String getBatch5A()
        {
            return fromA5;
        }
        public String getBatch6A()
        {
            return fromA6;
        }
        public String getTickersFromA()
        {
            return fromA1 + ',' + fromA2 + ',' + fromA3 + ',' + fromA4 + ',' + fromA5 + ',' + fromA6;
        }
        public String getTickersFromB()
        {
            return fromB1 + ',' + fromB2;
        }
        public String getTickersFromC()
        {
            return fromC1 + ',' + fromC2 + ',' + fromC3 + ',' + fromC4 + ',' + fromC5 + ',' + fromC6;
        }
        public String getTickersFromD()
        {
            return fromD1 + ',' + fromD2;
        }
        public String getTickersFromE()
        {
            return fromE1 + ',' + fromE2 + ',' + fromE3;
        }
        public String getTickersFromF()
        {
            return fromF1 + ',' + fromF2;
        }
        public String getTickersFromG()
        {
            return fromG1 + ',' + fromG2 + ',' + fromG3;
        }
        public String getTickersFromH()
        {
            return fromH1 + ',' + fromH2;
        }
        public String getTickersFromI()
        {
            return fromI1 + ',' + fromI2 + ',' + fromI3;
        }
        public String getTickersFromJ()
        {
            return fromJ;
        }
        public String getTickersFromK()
        {
            return fromK;
        }
        public String getTickersFromL()
        {
            return fromL1 + ',' + fromL2;
        }
        public String getTickersFromM()
        {
            return fromM1 + ',' + fromM2 + ',' + fromM3;
        }
        public String getTickersFromN()
        {
            return fromN1 + ',' + fromN2 + ',' + fromN3;
        }
        public String getTickersFromO()
        {
            return fromO1 + ',' + fromO2;
        }
        public String getTickersFromP()
        {
            return fromP1 + ',' + fromP2 + ',' + fromP3;
        }
        public String getTickersFromQ()
        {
            return fromQ;
        }
        public String getTickersFromR()
        {
            return fromR1 + ',' + fromR2 + ',' + fromR3;
        }
        public String getTickersFromS()
        {
            return fromS1 + ',' + fromS2 + ',' + fromS3 + ',' + fromS4;
        }
        public String getTickersFromT()
        {
            return fromT1 + ',' + fromT2;
        }
        public String getTickersFromU()
        {
            return fromU;
        }
        public String getTickersFromV()
        {
            return fromV1 + ',' + fromV2;
        }
        public String getTickersFromW()
        {
            return fromW;
        }
        public String getTickersFromX()
        {
            return fromX;
        }
        public String getTickersFromY()
        {
            return fromY;
        }
        public String getTickersFromZ()
        {
            return fromZ;
        }
    }
}

