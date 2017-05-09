using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace Lesson4_Project
{
    class SelectionQuery
    {
        public static void selectionQuery(int comboIndex1, int comboIndex2, int comboIndex3, int comboIndex4, int comboIndex5)
        {
            IMxDocument pMxDoc;
            pMxDoc = (IMxDocument)ArcMap.Application.Document;

            IMap pMap;
            pMap = pMxDoc.FocusMap;



            //****************************************************************************************//

            //Obtain county FeatureClass
            IEnumLayer pLayers;
            pLayers = pMap.Layers;
            ILayer pLayer;
            pLayer = pLayers.Next();
            while (pLayer != null)
            {
                if (pLayer.Name == "counties")
                {
                    break;
                }
                pLayer = pLayers.Next();
            }
            IFeatureLayer pFLayer;
            pFLayer = (IFeatureLayer)pLayer;
            IFeatureClass pFClass;
            pFClass = pFLayer.FeatureClass;




            //****************************************************************************************//

            //Obtain city FeatureClass
            IEnumLayer pLayersCities;
            pLayersCities = pMap.Layers;
            ILayer pLayerCity;
            pLayerCity = pLayersCities.Next();
            while (pLayerCity != null)
            {
                if (pLayerCity.Name == "cities")
                {
                    break;
                }
                pLayerCity = pLayersCities.Next();
            }
            IFeatureLayer pFLayerCity;
            pFLayerCity = (IFeatureLayer)pLayerCity;
            IFeatureClass pFClassCity;
            pFClassCity = pFLayerCity.FeatureClass;

            //****************************************************************************************//

            //Statements to determine operator for ComboBox 1
            string queryOperator1 = null;

            if (comboIndex1 == 0) //First selection

            {
                queryOperator1 = ">";
            }
            else //Second selection
            {
                queryOperator1 = "<";
            }


            //Statements to determine operator for ComboBox 2
            string queryOperator2 = null;

            if (comboIndex2 == 0)//First selection
            {
                queryOperator2 = ">";
            }
            else //Second selection
            {
                queryOperator2 = "<";
            }


            //Statements to determine operator for ComboBox 3
            string queryOperator3 = null;

            if (comboIndex3 == 0) //First selection
            {
                queryOperator3 = ">";
            }
            else //Second selection
            {
                queryOperator3 = "<";
            }


            //Statements to determine operator for ComboBox 4
            double queryOperator4;

            if (comboIndex4 == 0) //First selection
            {
                queryOperator4 = 1;
            }
            else //Second selection
            {
                queryOperator4 = 0;
            }


            //Statements to determine operator for ComboBox 5
            string queryOperator5 = null;

            if (comboIndex5 == 0) //First selection
            {
                queryOperator5 = ">=";
            }
            else //Second selection
            {
                queryOperator5 = "<=";
            }

            //****************************************************************************************//

            IQueryFilter pQueryFilter; //** Creating a new QueryFilter
            pQueryFilter = new QueryFilter();

            //Where clause based on user selection within ComboBoxes
            pQueryFilter.WhereClause = "NO_FARMS87" + queryOperator1 + 500 + " " + "AND" + " " + "POP_SQMILE" + queryOperator2 + 150 + 
                                        " " + "AND" + " " + "AGE_18_64" + queryOperator3 + 25000; //** Defining the WhereClause

            //Instantiate search cursor
            IFeatureCursor pFCursor;
            pFCursor = pFClass.Search(pQueryFilter, true);

            IFeature pFeature;
            pFeature = pFCursor.NextFeature();  //** Getting the first Feature

            IFeatureSelection pFSel;
            pFSel = (IFeatureSelection)pFLayer;  //** QI

            //Select features
            pFSel.SelectFeatures(pQueryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);

            //****************************************************************************************//

            //Create the selection set that will be used as
            //the geometry source in the bind geometry below
            ISelectionSet pSelSet;
            pSelSet = pFSel.SelectionSet;


            
            //Code below will merge the selected geometry together
            //to form one geometry that is used to select cities
            IEnumGeometry pEnumGeom;
            pEnumGeom = new EnumFeatureGeometry();

            IEnumGeometryBind pEnumGeomBind;
            pEnumGeomBind = (IEnumGeometryBind)pEnumGeom;
            pEnumGeomBind.BindGeometrySource(null, pSelSet);

            IGeometryFactory pGeomFactory;
            pGeomFactory = (IGeometryFactory)new GeometryEnvironment();

            IGeometry pGeom;
            pGeom = pGeomFactory.CreateGeometryFromEnumerator(pEnumGeom);

            //****************************************************************************************//

            ISpatialFilter pSpatialFilter;
            pSpatialFilter = new SpatialFilter();

            pSpatialFilter.Geometry = pGeom;  //** Setting equal to the selected counties shape
            pSpatialFilter.GeometryField = "SHAPE";
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;  //** Getting features contained by counties shape
            pSpatialFilter.WhereClause = "UNIVERSITY = " + queryOperator4 + "AND " + "CRIME_INDE" + queryOperator5 + 0.02;

            IFeatureSelection fSel;
            fSel = (IFeatureSelection)pFLayerCity;  //** QI

            fSel.SelectFeatures(pSpatialFilter, esriSelectionResultEnum.esriSelectionResultNew, false);

            //****************************************************************************************//

            //Clear selection before refreshing view
            pFSel.Clear();

            //Refresh dataframe view
            IActiveView pActiveView;
            pActiveView = (IActiveView)pMxDoc.FocusMap;
            pActiveView.Refresh();
        }
    }
}
