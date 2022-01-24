using CertificadorGuateFacturas.Model.Factura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CertificadorGuateFacturas
{
    public class Factura : Interfaces.IDocumento
    {
        //private string _url;
        //private string _option;
        //public Factura(string url, string option)
        //{
        //    _url = url;
        //    _option = option;
        //}
        public void Anular(Model.Credenciales credenciales, string xmlGenerado )
        {
            try
            {
                //Creando request
                com.guatefacturas.dte.Guatefac gtF = new com.guatefacturas.dte.Guatefac();
                var respuesta = gtF.generaDocumento(credenciales.PUsuario, credenciales.PPassword, credenciales.PNitEmisor, credenciales.PEstablecimiento, 
                                    credenciales.PTipoDoc, credenciales.PIdMaquina, credenciales.PTipoRespuesta, xmlGenerado);


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        public string ArmarXML(Model.Factura.Factura factura)
        {
            string xml = "";
            try
            {
                
                xml += @"<DocElectronico> \n";
                xml += @"   <Encabezado> \n";
                xml += @"      <Receptor> \n";
                xml += $"          <NITReceptor>{factura.receptor.NITRECEPTOR}</NITReceptor> \n";
                xml += $"          <Nombre>{factura.receptor.Nombre}</Nombre> \n";
                xml += $"          <Direccion>{factura.receptor.Direccion}</Direccion> \n";
                xml += @"      </Receptor> \n";
                xml += @"      <InfoDoc> \n";
                xml += $"          <TipoVenta>{factura.infoDoc.TipoVenta}</TipoVenta> \n";
                xml += $"          <DestinoVenta>{factura.infoDoc.DestinoVenta}</DestinoVenta> \n";
                xml += $"          <Fecha>{factura.infoDoc.Fecha}</Fecha> \n";
                xml += $"          <Moneda>{factura.infoDoc.Modeda}</Moneda> \n";
                xml += $"          <Tasa>{factura.infoDoc.Tasa}</Tasa> \n";
                xml += $"          <Referencia>{factura.infoDoc.Referencia}</Referencia> \n";
                xml += @"      </InfoDoc> \n";
                xml += @"      <Totales> \n";
                xml += $"          <Bruto>{factura.totalModel.Bruto}</Bruto> \n";
                xml += $"          <Descuento>{factura.totalModel.Descuento}</Descuento> \n";
                xml += $"          <Exento>{factura.totalModel.Exento}</Exento> \n";
                xml += $"          <Otros>{factura.totalModel.Otros}</Otros> \n";
                xml += $"          <Neto>{factura.totalModel.Neto}</Neto> \n";
                xml += $"          <Isr>{factura.totalModel.ISR}</Isr> \n";
                xml += $"          <Iva>{factura.totalModel.Iva}</Iva> \n";
                xml += $"          <Total>{factura.totalModel.Total}</Total> \n";
                xml += @"      </Totales> \n";
                xml += @"   </Encabezado> \n";

                xml += @"   <Detalles> \n";
               
                foreach(  Model.Factura.ProductosModel itemProdcuto in factura.productos)
                {
                xml += @"       <Productos> \n";
                xml += $"           <Producto> ${itemProdcuto.Producto}</Producto> \n";
                xml += $"           <Descripcion> ${itemProdcuto.Descripcion}</Descripcion> \n";
                xml += $"           <Medida> ${itemProdcuto.Medida}</Medida> \n";
                xml += $"           <Cantidad> ${itemProdcuto.Cantidad}</Cantidad> \n";
                xml += $"           <Precio> ${itemProdcuto.Precio}</Precio> \n";
                xml += $"           <PorcDescuento> ${itemProdcuto.PorcDesc}</PorcDescuento> \n";
                xml += $"           <ImpBruto> ${itemProdcuto.ImpBruto}</ImpBruto> \n";
                xml += $"           <ImpDescuento> ${itemProdcuto.ImpDescuento}</ImpDescuento> \n";
                xml += $"           <ImpExento> ${itemProdcuto.Precio}</ImpExento> \n";
                xml += $"           <ImpDescuento> ${itemProdcuto.ImpDescuento}</ImpDescuento> \n";
                xml += $"           <ImpExento> ${itemProdcuto.ImpExento}</ImpExento> \n";
                xml += $"           <ImpOtros> ${itemProdcuto.ImpOtros}</ImpOtros> \n";
                xml += $"           <ImpNeto> ${itemProdcuto.ImpNeto}</ImpNeto> \n";
                xml += $"           <ImpIsr> ${itemProdcuto.ImpIsr}</ImpIsr> \n";
                xml += $"           <ImpIva> ${itemProdcuto.ImpIva}</ImpIva> \n";
                xml += $"           <ImpTotal> ${itemProdcuto.ImpTotal}</ImpTotal> \n";
                xml += $"           <TipoVentaDet> ${itemProdcuto.TipoVentaDet}</TipoVentaDet> \n";
                xml += @"       </Productos> \n";
                }
                
                xml += @"   </Detalles> \n";
                xml += @"</DocElectronico> \n";


            }
            catch (Exception ex )
            {

                throw new Exception(ex.Message, ex.InnerException);
            }

            return xml;
        }

        public void Certificar(Model.Credenciales credenciales, string xmlGenerado)
        {
            try
            {
                //Creando request
                com.guatefacturas.dte.Guatefac gtF = new com.guatefacturas.dte.Guatefac();
                var respuesta = gtF.generaDocumento(credenciales.PUsuario, credenciales.PPassword, credenciales.PNitEmisor, credenciales.PEstablecimiento,
                                    credenciales.PTipoDoc, credenciales.PIdMaquina, credenciales.PTipoRespuesta, xmlGenerado);


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        private Model.Factura.Factura  obtenerDatos(SAPbobsCOM.ICompany B1company, int DocEntry)
        {
            try
            {
                Model.Factura.Factura factura = new Model.Factura.Factura();


                SAPbobsCOM.Recordset recorset = B1company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                string Query = $"EXEC FEL_GUATE_FAC({DocEntry})";
                recorset.DoQuery(Query);
                recorset.MoveFirst();
                while (!recorset.EoF)
                {
                    //oRecordset.Fields.Item(0).Value
                    //recorset.Fields.Item()
                    
                    //Receptor
                    factura.receptor.NITRECEPTOR = recorset.Fields.Item(0).Value;
                    factura.receptor.Nombre = recorset.Fields.Item(1).Value;
                    factura.receptor.Direccion = recorset.Fields.Item(2).Value;

                    //infoDOc
                    factura.infoDoc.TipoVenta = recorset.Fields.Item(3).Value;
                    factura.infoDoc.DestinoVenta = recorset.Fields.Item(4).Value;
                    factura.infoDoc.Fecha = recorset.Fields.Item(5).Value;
                    factura.infoDoc.Modeda = recorset.Fields.Item(6).Value;
                    factura.infoDoc.Tasa = recorset.Fields.Item(7).Value;
                    factura.infoDoc.Referencia = recorset.Fields.Item(8).Value;
                     
                    //Totales
                    factura.totalModel.Bruto = recorset.Fields.Item(9).Value;
                    factura.totalModel.Descuento = recorset.Fields.Item(10).Value;
                    factura.totalModel.Exento = recorset.Fields.Item(11).Value;
                    factura.totalModel.Otros = recorset.Fields.Item(12).Value;
                    factura.totalModel.Neto = recorset.Fields.Item(13).Value;
                    factura.totalModel.ISR = recorset.Fields.Item(14).Value;
                    factura.totalModel.Iva = recorset.Fields.Item(15).Value;
                    factura.totalModel.Total = recorset.Fields.Item(16).Value;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            

            return null;
        }

  
    }
}
