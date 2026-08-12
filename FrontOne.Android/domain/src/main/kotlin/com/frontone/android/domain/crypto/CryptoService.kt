package com.frontone.android.domain.crypto

import java.util.Base64
import javax.crypto.Cipher
import javax.crypto.SecretKeyFactory
import javax.crypto.spec.IvParameterSpec
import javax.crypto.spec.PBEKeySpec
import javax.crypto.spec.SecretKeySpec

/**
 * Réplica EXACTA de FrontOne.Shared/Security/CryptoService.cs (FrontOne WinForms) —
 * misma passphrase/salt/iteraciones/derivación de clave e IV. Necesario para poder
 * descifrar Seguridad.Usuario.PasswordEncriptado tal como lo guardó el escritorio
 * (es cifrado AES simétrico reversible, no un hash de una vía).
 *
 * DECISIÓN DE SEGURIDAD DOCUMENTADA (confirmada con el usuario, 2026-08-07): esto
 * empotra la misma clave maestra en el APK que ya vive hardcodeada en el ensamblado
 * de FrontOne.WinForms.exe — no es un riesgo nuevo que introduce Android, es el mismo
 * riesgo que ya existe hoy en el ejecutable de escritorio (ambos son igual de fáciles
 * de decompilar). Se decidió portar igual que escritorio por consistencia, en vez de
 * mover la validación a un SP del lado servidor (que hoy tampoco existe en WinForms).
 *
 * Únicamente `decrypt` está implementado — Android nunca necesita cifrar contraseñas
 * (eso solo pasa al dar de alta/editar un Usuario, función que sigue siendo exclusiva
 * de escritorio).
 *
 * Nunca cambiar estas constantes sin cambiar también las de CryptoService.cs — deben
 * quedar bit a bit idénticas o ningún password existente se podrá descifrar.
 */
object CryptoService {
    private const val PASSPHRASE = "FrontOne-ERP-Empacadora-Aguacate-2026"
    private const val SALT = "FrontOne-Salt-Fijo-v1"
    private const val KEY_SIZE_BITS = 256
    private const val ITERATIONS = 100_000

    fun decrypt(cifradoBase64: String): String {
        val (key, iv) = derivarClaveEIv()
        val cipher = Cipher.getInstance("AES/CBC/PKCS5Padding")
        cipher.init(Cipher.DECRYPT_MODE, SecretKeySpec(key, "AES"), IvParameterSpec(iv))
        val textoPlano = cipher.doFinal(Base64.getDecoder().decode(cifradoBase64))
        return String(textoPlano, Charsets.UTF_8)
    }

    /**
     * Rfc2898DeriveBytes.Pbkdf2(passphraseBytes, saltBytes, iterations, SHA256, keyBytes+16)
     * del lado C# — equivalente exacto vía PBKDF2WithHmacSHA256: se derivan
     * (32+16)=48 bytes, los primeros 32 son la clave AES-256, los últimos 16 son el IV.
     * Passphrase/salt son ASCII puro, así que char[] vs UTF-8 bytes no diverge.
     */
    private fun derivarClaveEIv(): Pair<ByteArray, ByteArray> {
        val factory = SecretKeyFactory.getInstance("PBKDF2WithHmacSHA256")
        val longitudTotalBits = (KEY_SIZE_BITS + 16 * 8)
        val spec = PBEKeySpec(PASSPHRASE.toCharArray(), SALT.toByteArray(Charsets.UTF_8), ITERATIONS, longitudTotalBits)
        val derivado = factory.generateSecret(spec).encoded
        val key = derivado.copyOfRange(0, KEY_SIZE_BITS / 8)
        val iv = derivado.copyOfRange(KEY_SIZE_BITS / 8, derivado.size)
        return key to iv
    }
}
