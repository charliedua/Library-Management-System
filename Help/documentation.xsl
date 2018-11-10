<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:variable name="myassembly">
<xsl:value-of select="doc/assembly/name"/>
</xsl:variable>
<xsl:template match="doc">
<HTML>
<HEAD>
<TITLE>
Visual Studio XML Documentation Stylesheet v20100201
</TITLE>
</HEAD>
<BODY>
<center>
<b>
<u>
<xsl:value-of select="$myAssembly"/>
documentation
</u>
</b>
</center>
<p/>
<xsl:apply-templates select="members"/>
</BODY>
</HTML>
</xsl:template>
<xsl:template match="members">
<xsl:apply-templates select="member"/>
</xsl:template>

<xsl:template match="member">

<xsl:variable name="myWholeName">
<xsl:value-of select="@name"/>
</xsl:variable>

<xsl:variable name="myClassMethodsArgs">
<xsl:value-of select="substring-after($myWholeName,concat($myAssembly,'.'))"/>
</xsl:variable>

<xsl:variable name="myClass">
<xsl:if test="contains($myClassMethodsArgs,'.')">
<xsl:value-of select="substring-before($myClassMethodsArgs,'.')"/>
</xsl:if>
<xsl:if test="not(contains($myClassMethodsArgs,'.'))">
<xsl:value-of select="$myClassMethodsArgs"/>
</xsl:if>
</xsl:variable>

<xsl:variable name="myMethodArgs">
<xsl:if test="contains($myClassMethodsArgs,'.')">
<xsl:value-of select="substring-after($myClassMethodsArgs,'.')"/>
</xsl:if>
<xsl:if test="not(contains($myClassMethodsArgs,'.'))">
<xsl:value-of select="$myClassMethodsArgs"/>
</xsl:if>
</xsl:variable>
<font color="red">
<xsl:value-of select="$myClass"/>
</font>
<br/>
<b>
<big>
<big>
<code>
  
<xsl:if test="contains($myWholeName,'M:') and contains($myWholeName,'(')">
<xsl:value-of select="$myMethodArgs"/>
</xsl:if>
<xsl:if test="contains($myWholeName,'M:') and not(contains($myWholeName,'('))">
<xsl:value-of select="concat($myMethodArgs,'()')"/>
</xsl:if>
<xsl:if test="not(contains($myWholeName,'M:'))">
<xsl:value-of select="$myMethodArgs"/>
</xsl:if>
</code>
</big>
</big>
</b>
<br/>
<xsl:apply-templates select="summary"/>
<xsl:apply-templates select="param"/>
<xsl:apply-templates select="returns"/>
<xsl:apply-templates select="exception"/>
<p/>
</xsl:template>
<xsl:template match="summary">

<i>* Summary: </i>
<xsl:value-of select="."/>
<br/>
</xsl:template>
<xsl:template match="param">

<i>* Param: </i>
<b>
<font color="blue">
<code>
<xsl:value-of select="@name"/>
</code>
</font>
:
</b>
<xsl:value-of select="."/>
<br/>
</xsl:template>
<xsl:template match="returns">
<i>* Returns: </i>
<xsl:value-of select="."/>
<br/>
</xsl:template>
<xsl:template match="exception">

<i>* Exception: </i>
(
<xsl:value-of select="@cref"/>
)
<xsl:value-of select="."/>
<br/>
</xsl:template>
</xsl:stylesheet>