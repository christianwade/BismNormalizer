   TO SET UP NEW DEVELOPMENT MACHINE 
   ---------------------------------

In project properities > Debug tab, set
- Start External Program: C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe
- Command Line Arguments: /rootsuffix Exp

Do a Release build from the command-line (see command in BismNormalizer.targets) to set up cross project references.

---
To install Microsoft.Office.Interop.Excel, run the following command in the Package Manager Console
    >Install-Package Microsoft.Office.Interop.Excel

To install Json.NET, run the following command in the Package Manager Console
    >Install-Package Newtonsoft.Json -Version 7.0.1

To install MSBuild.Extension.Pack, run the following command in the Package Manager Console
    >Install-Package MSBuild.Extension.Pack

May need to temporarily comment out <!--<Import Project="..\packages\MSBuild.Extension.Pack.1.8.0\build\net40\MSBuild.Extension.Pack.targets" />-->
to load project for 1st time.



   PROCESS IN CODE TO MANAGE TABLES / RELATIONSHIPS (Tom: CompatibilityLevel 1200)
   -------------------------------------------------------------------------------

1. Upon DeleteTable, delete all the table's associated relationships (child or parent)
2. Upon CreateTable, do not create any relationships from source
3. Upon UpdateTable, call DeleteTable returning all associated relationships as a backup from target.
    => CreateTable
    => Add back relationships from backup where possible (if from/to columns still exist).
4. Iterate Delete relationships.
5. Iterate Create relationships.
6. Call ValidateRelationships that will iterate all tables and check relationship paths for ambiguity. If found, set
   relationship that was copied from source to inactive.


   PROCESS IN CODE TO MANAGE TABLES / RELATIONSHIPS (Amo: CompatibilityLevel 1100 and 1300)
   ----------------------------------------------------------------------------------------

1. Start by deleting all reference dims in both dbs
2. Upon DeleteTable, delete all the table's child relationships
3. Upon CreateTable, do not create any parent relationships from source (or actually, delete them as cloning from source)
4. Upon UpdateTable, call Delete method (but this time not deleting the table's child relationships), then Create method (which
   will not create any parent relationships from source).  Then add back parent relationships from target clone (assuming
   necessary tables exist).
5. Iterate Delete relationships.
6. Iterate Create relationships.
7. Repopulate reference dims based on relationships.  When iterating the tables, keep a record (string array) of
   relationships added (or having ref dim implemented) that are active relationships.  If come across a 2nd one to the same
   table, the one that was already in the target wins.


    TEST CASES
    ----------

Test relationship with same internal name - by changing table names that were originially in both source and target

Test a .bsmn file that was migrated from old version with tabular model that was migrated to 1200.  Might have error with changed LongName to InternalName as serialized???

See if can have a table with different paritions from different connections

Pasted tables (with connections)

Test different possible routes to an active dimension (like via source gives you a different active relationship than via original target) - the one
from the target wins.

Test same table name different connection

Rename objects resulting in same name/different ids and vice versa
    - Change table name in target
    - Change table name in source
    - Test a table with a combination of relationships
        - some that will be valid after update child table, some won't
        - some that will be valid after update parent table, some won'
    - Same thing for connections and KPIs too (search for Guid.NewGuid()?)

Test connection different ID

Create measure that already exists in target, but in a different table

Create KPI that already exists in target, but in a different table

Blank db with no cube and/or mdx script

Relationship to non unique key column

Test creating a relationship between 2 tables where the parent attribute name is the same, but the id is different

Test reopening the BIM file in middle of comparison - does it invalidate comparison?  Do it multiple times!

Test checking out bim file that's under source control as part of BISM Norm update to a project
