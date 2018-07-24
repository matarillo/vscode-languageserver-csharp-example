/* --------------------------------------------------------------------------------------------
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 * ------------------------------------------------------------------------------------------ */
'use strict';

import * as path from 'path';
import * as os from 'os';
import { workspace, ExtensionContext } from 'vscode';

import {
	LanguageClient,
	LanguageClientOptions,
	ServerOptions,
	TransportKind
} from 'vscode-languageclient';

let client: LanguageClient;

export function activate(context: ExtensionContext) {
	// The server is implemented in C#
	let serverCommand = context.asAbsolutePath(
		path.join('server', 'out', 'SampleServer.exe')
	);

	let serverOptions: ServerOptions = (os.platform() === 'win32') ? {
		run: {
			command: serverCommand,
			transport: TransportKind.stdio
		},
		debug: {
			command: serverCommand,
			transport: TransportKind.stdio
		}
	} : {
		run: {
			command: 'mono',
			args: [serverCommand],
			transport: TransportKind.stdio
		},
		debug: {
			command: 'mono',
			args: [serverCommand],
			transport: TransportKind.stdio
		}
	};

	// Options to control the language client
	let clientOptions: LanguageClientOptions = {
		// Register the server for plain text documents
		documentSelector: [{ scheme: 'file', language: 'plaintext' }],
		synchronize: {
			// Notify the server about file changes to '.clientrc files contained in the workspace
			fileEvents: workspace.createFileSystemWatcher('**/.clientrc')
		}
	};

	// Create the language client and start the client.
	client = new LanguageClient(
		'languageServerExample',
		'Language Server Example',
		serverOptions,
		clientOptions
	);

	// Start the client. This will also launch the server
	client.start();
}

export function deactivate(): Thenable<void> {
	if (!client) {
		return undefined;
	}
	return client.stop();
}
