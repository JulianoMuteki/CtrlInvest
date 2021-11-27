INSERT INTO public."TicketSyncs"(
"TicketSyncID", "TickerID", "IsEnabled", "DateStart", "CreationDate", "DateModified", "IsDelete", "IsDisable")	
	VALUES (uuid_generate_v1(), (SELECT "Id" FROM public."Tickets"
WHERE "Ticker" = 'TAEE11.SA'), true, '2015-10-30 20:28:46.940105', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, false, false);

INSERT INTO public."TicketSyncs"(
"TicketSyncID", "TickerID", "IsEnabled", "DateStart", "CreationDate", "DateModified", "IsDelete", "IsDisable")	
	VALUES (uuid_generate_v1(), (SELECT "Id" FROM public."Tickets"
WHERE "Ticker" = 'VALE3.SA'), true, '2000-01-02 01:28:46.940105', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, false, false);