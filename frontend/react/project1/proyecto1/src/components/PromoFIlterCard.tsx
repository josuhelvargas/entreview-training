// src/components/PromoFilterCard.tsx
import React, { useEffect, useMemo, useState } from "react";

export type ParticipationType = {
  id: string;
  name: string;
  promoCodeRequired?: boolean;
};

export type PromoFilterCardProps = {
  participationTypes: ParticipationType[];
  status: "idle" | "loading" | "success" | "error";
};

export function PromoFilterCard({ participationTypes, status }: PromoFilterCardProps) {
  const [promotionCode, setPromotionCode] = useState("");
  const [accessCodeError, setAccessCodeError] = useState<string | null>(null);

  const visibleTypes = useMemo(() => {
    return participationTypes.filter(
      (p) => !promotionCode || p.promoCodeRequired === true,
    );
  }, [participationTypes, promotionCode]);

  const hasNoPromoResults =
    Boolean(promotionCode) && status === "success" && visibleTypes.length === 0;

  useEffect(() => {
    if (status !== "success") return;
    setAccessCodeError(hasNoPromoResults ? "Please enter a valid access code." : null);
  }, [hasNoPromoResults, status]);

  return (
    <section aria-label="promo-filter-card">
      <label>
        Access code
        <input
          aria-label="access-code"
          value={promotionCode}
          onChange={(e) => setPromotionCode(e.target.value)}
          placeholder="Enter code"
        />
      </label>

      {status === "loading" && <p role="status">Loading...</p>}

      {accessCodeError && <p role="alert">{accessCodeError}</p>}

      <ul aria-label="visible-types">
        {visibleTypes.map((t) => (
          <li key={t.id}>{t.name}</li>
        ))}
      </ul>
    </section>
  );
}
