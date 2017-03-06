namespace bUtility.Dapper.Test
{
    public class PeculiarTestClass
    {
        //it is a calculated column in the DB (eg identifier in SqlServer)
        public int? id { get; set; }
        public string channel { get; set; }
        //purposely using a reserved word
        public int? index { get; set; }
    }
}


/*  SqlServer query to create table PeculiarTestClass
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE[dbo].[PeculiarTestClass](
    [id]        [int] IDENTITY(1,1) NOT NULL,
    [channel]   [varchar](50)       NULL,
    [index]     [int]               NULL
) ON[PRIMARY]

GO

SET ANSI_PADDING OFF
GO

*/
