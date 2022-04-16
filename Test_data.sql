INSERT INTO public."TicketSyncs"(
"TicketSyncID", "TickerID", "IsEnabled", "DateStart", "CreationDate", "DateModified", "IsDelete", "IsDisable")	
	VALUES (uuid_generate_v1(), (SELECT "Id" FROM public."Tickets"
WHERE "Ticker" = 'TAEE11.SA'), true, '2015-10-30 20:28:46.940105', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, false, false);

INSERT INTO public."TicketSyncs"(
"TicketSyncID", "TickerID", "IsEnabled", "DateStart", "CreationDate", "DateModified", "IsDelete", "IsDisable")	
	VALUES (uuid_generate_v1(), (SELECT "Id" FROM public."Tickets"
WHERE "Ticker" = 'VALE3.SA'), true, '2000-01-02 01:28:46.940105', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, false, false);

INSERT INTO public."TicketSyncs"(
"TicketSyncID", "TickerID", "IsEnabled", "DateStart", "CreationDate", "DateModified", "IsDelete", "IsDisable")	
	VALUES (uuid_generate_v1(), (SELECT "Id" FROM public."Tickets"
WHERE "Ticker" = 'WEGE3.SA'), true, '2000-01-01', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, false, false);

INSERT INTO public."TicketSyncs"(
"TicketSyncID", "TickerID", "IsEnabled", "DateStart", "CreationDate", "DateModified", "IsDelete", "IsDisable")	
	VALUES (uuid_generate_v1(), (SELECT "Id" FROM public."Tickets"
WHERE "Ticker" = 'ABEV3.SA'), true, '2000-01-01', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, false, false);

INSERT INTO public."TicketSyncs"(
"TicketSyncID", "TickerID", "IsEnabled", "DateStart", "CreationDate", "DateModified", "IsDelete", "IsDisable")	
	VALUES (uuid_generate_v1(), (SELECT "Id" FROM public."Tickets"
WHERE "Ticker" = 'ITSA4.SA'), true, '2000-01-01', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, false, false);

INSERT INTO public."TicketSyncs"(
"TicketSyncID", "TickerID", "IsEnabled", "DateStart", "CreationDate", "DateModified", "IsDelete", "IsDisable")	
	VALUES (uuid_generate_v1(), (SELECT "Id" FROM public."Tickets"
WHERE "Ticker" = 'ITUB3.SA'), true, '2000-01-01', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, false, false);

INSERT INTO public."TicketSyncs"(
"TicketSyncID", "TickerID", "IsEnabled", "DateStart", "CreationDate", "DateModified", "IsDelete", "IsDisable")	
	VALUES (uuid_generate_v1(), (SELECT "Id" FROM public."Tickets"
WHERE "Ticker" = 'GGBR4.SA'), true, '2000-01-01', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, false, false);


INSERT INTO public."Banks"(
	"BankID", "Name", "BankBalance", "BankCode", "InitialBalance", "CreationDate", "DateModified", "IsDelete", "IsDisable")
	VALUES ('e069604c-70af-11ec-914b-0242ac130002', 'INTER DTVM LTDA', '77', 0, 0, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, false, false);


INSERT INTO public."Banks"(
	"BankID", "Name", "BankBalance", "BankCode", "InitialBalance", "CreationDate", "DateModified", "IsDelete", "IsDisable")
	VALUES ('fe616efa-70af-11ec-914b-0242ac130002', 'XP INVESTIMENTOS CCTVM S/A', '102', 0, 0, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, false, false);