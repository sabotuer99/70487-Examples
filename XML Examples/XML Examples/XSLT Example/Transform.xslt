<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  
    <xsl:output method="html" indent="yes"/>
    <xsl:param name="source" select="'Generic'" />
    <xsl:template name="html_table" match="/">
      <html>
        <header>
          <style>
            td {
                border: 1px solid #dddddd;
                text-align: left;
                padding: 8px;
            }
          </style>
        </header>
        <body>
          <table>
            <thead>
              <tr>
                <th>Auction Platform</th>
                <th>Seller Name</th>
                <th>High Bidder Name</th>
                <th>Highest Bid Amount</th>
              </tr>
            </thead>
            <tbody>
              <xsl:for-each select="//listing">
                <tr>
                  <td><xsl:value-of select="$source"/></td>
                  <td><xsl:value-of select="seller_info/seller_name"/></td>
                  <td><xsl:value-of select="auction_info/high_bidder/bidder_name"/></td>
                  <td><xsl:value-of select="bid_history/highest_bid_amount" /></td>
                </tr>
              </xsl:for-each>
            </tbody>
          </table>
        </body>
      </html>
    </xsl:template>
</xsl:stylesheet>
