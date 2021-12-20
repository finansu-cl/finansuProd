function esEmail(campo,alertas,vacio,autofoco)
{	

	campo.value = campo.value.replace (/[ ]+$/,"");    					//realiza un rtrim...
	campo.value = campo.value.replace (/^[ ]+/,"");    					//realiza un ltrim...
	if (campo.value=="" && ! vacio)	return true;

	if (campo.value=="")
	{
	    if (alertas) alert("Debe ingresar una direcci\u00f3n e-mail.");
		if (autofoco) campo.focus();
		return false;
	}

	if (/(.*);(.+)/.test(campo.value) || /(.*),(.+)/.test(campo.value))		//valida que no haya mas de una direccion
	{
	    if (alertas) alert("S&oacute;lo puede ingresar una direcci\u00f3n e-mail.");
		if (autofoco) campo.focus();
		return false;
	}

   //if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,4})+$/.test(campo.value))	//valida el email
	if (/^\w+([\.-]?[\w-]+)*@\w+([\.-]?[\w-]+)*(\.\w{2,4})+$/.test(campo.value))	//valida el email-05/12/2005
	{
		return true;
	}
	else 
	{
	    if (alertas) alert("La direcci\u00f3n e-mail es incorrecta.");
		if (autofoco) campo.focus();
		return false;
	}
}

function esNumero(campo,nombre,alertas,vacio,autofoco)
{	
	campo.value = campo.value.replace (/[ ]+$/,"");    					//realiza un rtrim...
	campo.value = campo.value.replace (/^[ ]+/,"");    					//realiza un ltrim...
	if (campo.value=="" && ! vacio)	return true;

	if (campo.value=="")
	{
		if (alertas)  alert("Debe ingresar un valor numérico en el campo " + nombre + ".");
		if (autofoco) campo.focus();
		return false;
	}

	if (/(.*);(.+)/.test(campo.value) || /(.*),(.+)/.test(campo.value))		//valida que no haya mas de una direccion
	{
		if (alertas) alert("Sólo puede ingresar un valor en el campo " + nombre +".");
		if (autofoco) campo.focus();
		return false;
	}

	if (/^\d+$/.test(campo.value))	//valida el email
	{
		return true;
	}
	else 
	{
		if(alertas) alert("El campo " + nombre + " sólo puede tener números.");
		if (autofoco) campo.focus();
		return false;
	}
}

function esTexto(campo,nombre,alertas,vacio,autofoco)
{	
	campo.value = campo.value.replace (/[ ]+$/,"");    					//realiza un rtrim...
	campo.value = campo.value.replace (/^[ ]+/,"");    					//realiza un ltrim...
	if (campo.value=="" && ! vacio)	return true;

	if (campo.value=="")
	{
		if (alertas)  alert("Debe ingresar un valor en el campo " + nombre + ".");
		if (autofoco) campo.focus();
		return false;
	}
	else
	{
		return true;
	}
}

function esCelular(campo, nombre, alertas, vacio, autofoco, retorno) {
    campo.value = campo.value.replace(/[.]+/g, "");    					//reemplaza puntos...
    campo.value = campo.value.replace(/[-]+/g, "");    					//reemplaza guiones...
    campo.value = campo.value.replace(/[ ]+/g, "");    					//reemplaza espacios...
    campo.value = campo.value.replace(/^\+56/g, "");  					 //reemplaza codigopais y ciudad...

    if (campo.value == "" && !vacio) {
        return true;
    }
    if (campo.value == "") {
        if (alertas) {
            alert("Debe ingresar un " + nombre + ".");
        }
        //if (autofoco) retorno.focus();
        return false;
    }
    if (/(.*);(.+)/.test(campo.value) || /(.*),(.+)/.test(campo.value))		//valida que no haya mas de una direccion
    {
        if (alertas) {
            alert("Sólo puede ingresar un " + nombre + ".");
        }
        //if (autofoco) retorno.focus();
        return false;
    }
    if (/^(\d{8})$/.test(campo.value) && /^[5-9]/.test(campo.value))	//agrega 0 cuando falte desde el 6 al 9
    {
        campo.value = "0" + campo.value;
    }
    if (/^(\d{9})$/.test(campo.value) && /^[9]/.test(campo.value))	//agrega 0 cuando falte
    {
        campo.value = "0" + campo.value;
    }
    if (/^0[5-9](\d{7})$/.test(campo.value))	//valida el celular con 07, 08 y 09
    {
        campo.value = campo.value.replace(/^[0]+/, "");
        return true;
    }
    else {
        if (/^0[5-9](\d{8})$/.test(campo.value)) {
            return true;
        }
        else {
            if (alertas)
                alert("El " + nombre + " ingresado es incorrecto.");
            //retorno.value = "";
//            if (autofoco)
//                retorno.focus();
            return false;
        }
    }
}

function soloNum(Event, id) {
    tecla = (document.getElementById(id)) ? Event.keyCode : Event.which;
    if (tecla == 8) return true;
    //patron = /\d/;
    patron = /^[0-9.,]+$/;
    te = String.fromCharCode(tecla);
    return patron.test(te);
}

function soloNumSinPuntos(Event, id) {
    tecla = (document.getElementById(id)) ? Event.keyCode : Event.which;
    if (tecla == 8) return true;
    patron = /\d/;
    //patron = /^[0-9.,]+$/;
    te = String.fromCharCode(tecla);
    return patron.test(te);
}

function esTelefono(campo,nombre,alertas,vacio,autofoco)
{	
	campo.value = campo.value.replace (/[.]+/g,"");    					//reemplaza puntos...
	campo.value = campo.value.replace (/[-]+/g,"");    					//reemplaza guiones...
	campo.value = campo.value.replace (/[ ]+/g,"");    					//reemplaza espacios...
	campo.value = campo.value.replace (/^\+56/g,"");    				//reemplaza codigopais y ciudad...
	
	if (campo.value=="" && ! vacio)	return true;
	
	if (campo.value=="")
	{
		if (alertas)  alert("Debe ingresar un " + nombre + ".");
		if (autofoco) campo.focus();
		return false;
	}

	if (/(.*);(.+)/.test(campo.value) || /(.*),(.+)/.test(campo.value))		//valida que no haya mas de una direccion
	{
		if (alertas) alert("Sólo puede ingresar un " + nombre + ".");
		if (autofoco) campo.focus();
		return false;
	}

	if (/^(\d{7})$/.test(campo.value))	//agrega 2 cuando falte para telefonos de 7 digitos (santiago)
	{
		campo.value = "02"+campo.value;
	}
		
	if ( (/^02(\d{7})$/.test(campo.value)) || (/^[3-9][0-9](\d{6})$/.test(campo.value)) )	//valida el teléfono con codigo de ciudad
	{
		return true;
	}
	else 
	{
		if(alertas) alert("El " + nombre + " ingresado es incorrecto, recuerde ingresar código de área y teléfono.");
		if (autofoco) campo.focus();
		return false;
	}
}

function formatomiles(idcampo) {
    var form = document.getElementById(idcampo);
    var i = 0, j = 0, cnt = 0, largo = 0;
    var x = form.value.split(',');
    var etexto = new String(""), xtexto = new String(""), dtexto = new String(x[0]);

    largo = dtexto.length;
    // ciclo que da vuelta el string
    for (i = largo; i >= 0; i--) {
        if (dtexto.charAt(i) >= "0" && dtexto.charAt(i) <= "9") {
            etexto = etexto + dtexto.charAt(i);
        }
    }
    //ciclo que agrega puntos			
    for (i = 0; i <= largo; i++) {
        if (cnt == 3) {
            xtexto = xtexto + '.';
            xtexto = xtexto + etexto.charAt(i, 1);
            cnt = 1;
        }
        else {
            xtexto = xtexto + etexto.charAt(i, 1);
            cnt++;
        }
    }
    //ciclo que da vuelta el string formateado
    largo = xtexto.length;
    etexto = "";
    for (i = largo + 1; i >= 0; i--) {
        etexto = etexto + xtexto.charAt(i, 1);
    }
    if (etexto.charAt(0, 1) == '.') {
        largo = etexto.length;
        dtexto = etexto;
        etexto = "";
        for (i = 1; i <= largo; i++) {
            etexto = etexto + dtexto.charAt(i, 1);
        }
    }
    var y = x[1];
    if (x[1] == "undefined" || x[1] == null || x[1] == "") {
       y = "00";
    }

    form.value = etexto + "," + y;
}

function postAjax(data, url, id)
{
    var success = function (data) {
        data = $.parseJSON(data);
        $.each(data, function () {
            alert(data);
            //$("#" + id).val(data.total);
        });
    };

    if (data != "") {
        $.ajax({
            type: 'POST',
            url: url,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            crossDomain: true,
            cache: false,
            success: success,
            error: function (jqXHR, textStatus, errorThrown) {
                location.reload();
            }
        });
    } else
    {
        $.ajax({
            type: 'GET',
            url: url,
            //data: data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            crossDomain: true,
            cache: false,
            success: success,
            error: function (jqXHR, textStatus, errorThrown) {
                location.reload();
            }
        });

    }
}