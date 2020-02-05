Sub series()

Dim i



For i = 2 To 21960
Dim Lineas() As String
 
Dim raw
 raw = Range("d" & i).Value
 
 Range("j" & i).Value = Range("c" & i)
 
 Lineas = Split(raw, "Folio=""")
 Lin = Split(Lineas(1), """")
 
 Range("k" & i).Value = Lin(0)
 
 Lineas = Split(raw, "Serie=""")
 Lin = Split(Lineas(1), """")
 
 Range("l" & i).Value = Lin(0)
 
 Range("m" & i).Value = Range("k" & i).Value & Range("l" & i).Value
 
 

Next i


End Sub


/*

I was trying to read a XML string of a excel cell to stract 2 attributes but at the end,
but I surrendered and I decided to apply this fix.

This remainds me that I need to study more about XML because it is not easy like c# or Java.



*/